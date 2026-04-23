using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Podcasts.GetAll;

public class GetAllPodcastsHandler : IRequestHandler<GetAllPodcastsQuery, List<PodcastDTO>>
{
    private readonly ICrudRepository<Podcast> _podcastRepository;

    public GetAllPodcastsHandler(ICrudRepository<Podcast> podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<List<PodcastDTO>> Handle(GetAllPodcastsQuery request, CancellationToken cancellationToken)
    {
        // Hent alle podcasts
        var podcasts = await _podcastRepository.GetAllAsync();

        // Hvis der ikke findes nogen podcasts, returner en tom liste
        if (podcasts == null || !podcasts.Any())
        {
            return new List<PodcastDTO>();
        }

        // Map til DTO'er
        var podcastDTOs = podcasts.Select(p => new PodcastDTO
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description
        }).ToList();

        return podcastDTOs;
    }
}