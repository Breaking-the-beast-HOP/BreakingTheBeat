using System.Text.Json;
using MediatR;
using MusicCatalogContext.Domain;
using MusicCatalogContext.Domain.Enums;
using SharedKernel.Interfaces;

namespace MusicCatalogContext.Features.MusicRelease.Withdraw;

public class WithdrawReleaseHandler : IRequestHandler<WithdrawReleaseCommand, bool>
{
    private readonly IReleaseRepository _releaseRepository;
    private readonly IOutBoxRepository _outboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WithdrawReleaseHandler(
        IReleaseRepository releaseRepository,
        IOutBoxRepository outboxRepository,
        IUnitOfWork unitOfWork)
    {
        _releaseRepository = releaseRepository;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(WithdrawReleaseCommand request, CancellationToken cancellationToken)
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

            if (release.Status is not (ReleaseStatus.ReadyForQc or ReleaseStatus.InQc))
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return false;
            }

            release.WithdrawFromQc(); // sætter Status = Draft

            var outboxMessage = OutboxMessage.Create(
                "ReleaseWithdrawnFromQc",
                JsonSerializer.Serialize(new
                {
                    ReleaseId = release.Id,
                    release.Title
                }));

            await _outboxRepository.AddAsync(outboxMessage);
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