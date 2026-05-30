using Gateway.Contracts.Podcast.Contribution;

namespace Gateway.Contracts.Podcast;

public class NewPodcastRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public Guid CategoryId { get; set; }
    public Guid LanguageId { get; set; }

    public List<ContributionRequest>? Contributions { get; set; }

    public List<NewPodcastSeasonRequest> Seasons { get; set; } = new();
    
}

public class NewPodcastSeasonRequest
{
    public string? Title { get; set; }
    public int SeasonNumber { get; set; }

    public List<NewPodcastEpisodeRequest> Episodes { get; set; } = new();
}

public class NewPodcastEpisodeRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }

    public TimeSpan? Duration { get; set; }
    public DateTime? AiringDate { get; set; }

    public Guid? TagId { get; set; }

    public string Status { get; set; } = "Draft";
    public List<ContributionRequest>? Contributions { get; set; }
}