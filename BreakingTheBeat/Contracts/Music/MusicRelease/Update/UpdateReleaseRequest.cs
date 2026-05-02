using Gateway.Contracts.Music.Contribution;

namespace Gateway.Contracts.Music.MusicRelease.Update
{
    public class UpdateReleaseRequest
    {
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public Guid? GenreId { get; set; }
        public Guid? LanguageId { get; set; }


        public List<UpdateContributionRequest> Contributions { get; set; } = new();
        public List<UpdateVolumeRequest> Volumes { get; set; } = new();
    }
}