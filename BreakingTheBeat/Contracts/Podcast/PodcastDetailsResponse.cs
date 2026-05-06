namespace Gateway.Contracts.Podcast
{
    public class PodcastDetailsResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public Guid LanguageId { get; set; }
        public string? LanguageName { get; set; }

        public List<ContributionResponse> Contributions { get; set; } = new();
    }

    public class ContributionResponse
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string PersonName { get; set; } = null!;
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}