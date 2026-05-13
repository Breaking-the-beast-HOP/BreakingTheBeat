namespace PodcastCatalog.Features.Contributors.GetAll
{
    public class ContributorDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
