using PodcastCatalog.Domain.Entities;

namespace PodcastCatalog.Features.Podcasts.Shared
{
    public static class PodcastMappings
    {
        public static PodcastDTO ToDto(this Podcast podcast)
        {
            return new PodcastDTO
            {
                Id = podcast.Id,
                Title = podcast.Title,
                Description = podcast.Description,
                CategoryId = podcast.CategoryId,
                CategoryName = podcast.Category?.Name,
                LanguageId = podcast.LanguageId,
                LanguageName = podcast.Language?.Name,

                Contributions = podcast.Contributions
                    .Select(c => new ContributionRowDTO
                    {
                        Id = c.Id,
                        ContributorId = c.ContributorId,
                        ContributorName = c.Contributor.Name,
                        ContributorRoleId = c.ContributorRoleId,
                        ContributorRoleName = c.Role.Name,
                        HasRights = c.HasRights
                    })
                    .ToList()
            };
        }
    }
}
