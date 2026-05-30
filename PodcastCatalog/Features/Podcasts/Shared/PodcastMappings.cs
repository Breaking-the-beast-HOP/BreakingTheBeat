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
                        ContributorName = c.Contributor?.DisplayName,
                        ContributorRoleId = c.ContributorRoleId,
                        ContributorRoleName = c.Role?.Name,
                        HasRights = c.HasRights
                    })
                    .ToList(),

                Seasons = podcast.Seasons
                    .OrderBy(s => s.SeasonNumber)
                    .Select(s => new SeasonDTO
                    {
                        Id = s.Id,
                        Title = s.Title,
                        SeasonNumber = s.SeasonNumber,
                        ReleaseDate = s.ReleaseDate,

                        Episodes = s.Episodes
                            .OrderBy(e => e.EpisodeNumber)
                            .Select(e => new EpisodeDTO
                            {
                                Id = e.Id,
                                Title = e.Title,
                                Description = e.Description,
                                Duration = e.Duration,
                                AiringDate = e.AiringDate,
                                TagId = e.TagId,
                                TagName = e.EpisodeTag?.Name,
                                EpisodeNumber = e.EpisodeNumber,
                                Status = e.Status
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}
