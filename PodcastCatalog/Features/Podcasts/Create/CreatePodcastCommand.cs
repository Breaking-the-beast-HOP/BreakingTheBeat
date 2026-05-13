using MediatR;
using PodcastCatalog.Features.Podcasts.Shared;

namespace PodcastCatalog.Features.Podcasts.Create
{
    public record CreatePodcastCommand(
                                string Title, 
                                string Description, 
                                Guid CategoryId, 
                                Guid LanguageId, 
                                List<CreateContributionDTO> Contributors, 
                                List<CreateSeasonDTO> Seasons
        ) : IRequest<PodcastDTO>;

    public class CreateSeasonDTO
    {
        public string? Title { get; set; }
        public int SeasonNumber { get; set; }
        public List<CreateEpisodeDTO> Episodes { get; set; } = new();
    }

    public class CreateEpisodeDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? AiringDate { get; set; }
        public Guid? TagId { get; set; }
        public string Status { get; set; } = "Draft";
    }
}
