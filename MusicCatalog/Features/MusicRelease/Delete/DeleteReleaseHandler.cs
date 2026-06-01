using System.Text.Json;
using MediatR;
using MusicCatalogContext.Domain;
using MusicCatalogContext.Domain.Enums;
using SharedKernel.Interfaces;

namespace MusicCatalogContext.Features.MusicRelease.Delete;

public class DeleteReleaseHandler : IRequestHandler<DeleteReleaseCommand, bool>
{
    private readonly IReleaseRepository _releaseRepository;
    private readonly IOutBoxRepository _outboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReleaseHandler(
        IReleaseRepository releaseRepository,
        IOutBoxRepository outboxRepository,
        IUnitOfWork unitOfWork)
    {
        _releaseRepository = releaseRepository;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteReleaseCommand request, CancellationToken cancellationToken)
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

            if (release.Status != ReleaseStatus.Draft) //because a draft never left the catalog, we don't need to publish an event about it being deleted
            {
                var outboxMessage = OutboxMessage.Create(
                    "ReleaseDeleted",
                    JsonSerializer.Serialize(new
                    {
                        ReleaseId = release.Id,
                        release.Status
                    }));

                await _outboxRepository.AddAsync(outboxMessage);
            }

            var deleted = await _releaseRepository.DeleteAsync(request.Id);

            if (!deleted)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return false;
            }

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