namespace Gateway.Contracts.Music.MusicRelease.Get
{

    public class ReleaseResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public string? GenreName { get; set; }
        public string? LanguageName { get; set; }
    }
}

