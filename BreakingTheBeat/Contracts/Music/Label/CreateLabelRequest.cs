namespace Gateway.Contracts.Music.Label
{
    public class CreateLabelRequest
    {
        public string Name { get; set; } = string.Empty;

        public string LabelCode { get; set; } = string.Empty;

        public string? Website { get; set; }

        public string? InstagramUrl { get; set; }

        public string? TikTokUrl { get; set; }

        public string? YouTubeUrl { get; set; }

        public string? FacebookUrl { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? PrimaryLanguageId { get; set; }

        public string? LogoUrl { get; set; }

        public CreateLabelContactPersonRequest ContactPerson { get; set; } = new();
    }

}