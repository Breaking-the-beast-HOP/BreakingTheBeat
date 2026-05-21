namespace Frontend.Components.Pages.MusicQualityControl
{
    public class QcReleaseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Ean { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public Guid ReleaseId { get; set; }

        public DateTime SubmittedAt { get; set; }

        public string? LatestQcNote { get; set; }

        public QcReleaseDto(
         Guid releaseId,
         string title,
         string artist,
         string type,
         string ean,
         string status,
         DateTime submittedAt,
         string? latestQcNote)
        {
            Id = Guid.NewGuid();
            ReleaseId = releaseId;
            Title = title;
            Artist = artist;
            Type = type;
            Ean = ean;
            Status = status;
            SubmittedAt = submittedAt;
            LatestQcNote = latestQcNote;
        }
    }
}