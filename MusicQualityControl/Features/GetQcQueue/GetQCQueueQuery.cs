using MediatR;
using MusicQualityControl.Domain.ReadModels;

namespace MusicQualityControl.Features.GetQcQueue;

public record GetQcQueueQuery() : IRequest<List<ReleaseQcQueueItem>>;