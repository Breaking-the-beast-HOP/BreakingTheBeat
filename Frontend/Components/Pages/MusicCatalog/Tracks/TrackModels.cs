using Frontend.Components.Pages.MusicCatalog.SharedModels;

namespace Frontend.Components.Pages.MusicCatalog.Tracks.Models;

public class TrackModel
{
    public string? Title { get; set; }
    public string? Version { get; set; }
    public string? ISRC { get; set; }

    public Guid? GenreId { get; set; }
    public Guid? MetadataLanguageId { get; set; }
    public Guid? AudioLanguageId { get; set; }

    public List<ContributionRequest> Contributions { get; set; } = new();
}

public class TrackReleaseDto
{
    public string? Title { get; set; }
}