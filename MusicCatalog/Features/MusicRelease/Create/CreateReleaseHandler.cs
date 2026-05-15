using System.Text.Json;
using MediatR;
using MusicCatalog.Domain;
using MusicCatalog.Domain.Enums;
using MusicCatalog.Infrastructure.Repositories;
using SharedKernel.Interfaces;

namespace MusicCatalog.Features.Release.Create
{
    public class CreateReleaseHandler : IRequestHandler<CreateReleaseCommand, Guid>
    {
        private readonly IReleaseRepository _releaseRepository;
        private readonly IOutBoxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReleaseHandler(IReleaseRepository releaseRepository, IOutBoxRepository outBoxRepository, IUnitOfWork unitOfWork)
        {
            _releaseRepository = releaseRepository;
            _outboxRepository = outBoxRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateReleaseCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var release = Domain.Entities.Release.Create(
                    request.Title,
                    request.Version,
                    request.EAN,
                    request.ReleaseDate,
                    request.GenreId,
                    request.LanguageId
                );

                foreach (var volumeRequest in request.Volumes.OrderBy(v => v.VolumeNumber))
                {
                    var volume = release.AddVolume(volumeRequest.VolumeNumber);

                    foreach (var trackRequest in volumeRequest.Tracks.OrderBy(t => t.TrackNumber))
                    {
                        volume.AddTrack(
                            trackRequest.TrackId,
                            trackRequest.TrackNumber
                        );
                    }
                }

                foreach (var contribution in request.Contributions)
                {
                    foreach (var roleId in contribution.RoleIds)
                    {
                        release.AddContribution(contribution.ContributorId, roleId);
                    }
                }

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
                            release.EAN,
                        }));

                    await _outboxRepository.AddAsync(outboxMessage);
                }

                await _releaseRepository.AddAsync(release);

                await _unitOfWork.CommitAsync(cancellationToken);

                return release.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}