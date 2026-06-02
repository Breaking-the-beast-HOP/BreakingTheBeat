using MediatR;

namespace MusicCatalogContext.Features.MusicRelease.Withdraw;

public record WithdrawReleaseCommand(Guid Id) : IRequest<bool>;