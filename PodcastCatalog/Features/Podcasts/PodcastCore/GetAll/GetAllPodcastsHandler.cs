using MediatR;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Features.Podcasts.Interfaces;
using PodcastCatalog.Features.Podcasts.Shared;
using SharedKernel;

namespace PodcastCatalog.Features.Podcasts.PodcastCore.GetAll;

public class GetAllPodcastsHandler : IRequestHandler<GetAllPodcastsQuery, List<PodcastDTO>>
{
    private readonly IPodcastRepository _podcastRepository;

    public GetAllPodcastsHandler(IPodcastRepository podcastRepository)
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
            return new List<PodcastDTO>
            {
                new PodcastDTO
                {
                    Title = "Ingen podcasts fundet, men her en til pynt",
                    Description = "Repository returnerede ingen podcasts. denne her er ikke persisteret i databasen så nej du kan ikke lave get by id på den, men du kan se listen ",

                }
            };

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