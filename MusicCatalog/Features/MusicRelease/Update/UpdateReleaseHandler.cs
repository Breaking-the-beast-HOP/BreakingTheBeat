using System.Text.Json;
using MediatR;
using MusicCatalogContext.Domain;
using MusicCatalogContext.Domain.Enums;
using SharedKernel.Interfaces;

namespace MusicCatalogContext.Features.MusicRelease.Update
{
    public class UpdateReleaseHandler : IRequestHandler<UpdateReleaseCommand, bool>
    {
        private readonly IReleaseRepository _releaseRepository;
        private readonly IOutBoxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReleaseHandler(
            IReleaseRepository releaseRepository,
            IOutBoxRepository outboxRepository,
            IUnitOfWork unitOfWork)
        {
            _releaseRepository = releaseRepository;
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateReleaseCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var release = await _releaseRepository.GetByIdAsync(request.Id);

                if (release is null)
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    return false;
                }

                release.UpdateDetails(
                    request.Title,
                    request.Version,
                    request.EAN,
                    request.GenreId,
                    request.MetadataLanguageId
                );

                release.UpdateContributions(
                    request.Contributions
                        .Select(c => (c.ContributorId, c.ContributorRoleId))
                        .ToList()
                );

                release.UpdateVolumes(
                    request.Volumes
                        .Select(v => (
                            VolumeNumber: v.VolumeNumber,
                            Tracks: v.Tracks
                                .Select(t => (
                                    TrackId: t.TrackId,
                                    TrackNumber: t.TrackNumber
                                ))
                                .ToList()
                        ))
                        .ToList()
                );

                if (release.Status != ReleaseStatus.ReadyForQc) //Makes sure we dont send the outbox message again for a already submitted release
                {
                    if (request.Status == ReleaseStatus.ReadyForQc)
                    {
                        release.SubmitToQc();

                        var outboxMessage = OutboxMessage.Create(
                            "ReleaseSubmittedToQc",
                            JsonSerializer.Serialize(new
                            {
                                ReleaseId = release.Id,
                                release.Title,
                                release.Version,
                                release.EAN
                            }));

                        await _outboxRepository.AddAsync(outboxMessage);
                    }
                }
                

                await _releaseRepository.UpdateAsync(release);

                await _unitOfWork.CommitAsync(cancellationToken);

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}

