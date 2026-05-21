using MediatR;

namespace MusicCatalogContext.Domain.Entities
{
    public class Label
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public string LabelCode { get; private set; }

        public string? Website { get; private set; }

        public string? InstagramUrl { get; private set; }
        public string? TikTokUrl { get; private set; }
        public string? YouTubeUrl { get; private set; }
        public string? FacebookUrl { get; private set; }

        public Guid? CountryId { get; private set; }
        public Country Country { get; private set; }

        public Guid? PrimaryLanguageId { get; private set; }
        public Language? PrimaryLanguage { get; private set; }

        public string? LogoUrl { get; private set; }

        public LabelContactPerson ContactPerson { get; private set; }

        private readonly List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        private void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        private Label() { }

        public static Label Create(
            string name,
            string labelCode,
            string? website,
            string? instagramUrl,
            string? tikTokUrl,
            string? youTubeUrl,
            string? facebookUrl,
            Guid? countryId,
            Guid? primaryLanguageId,
            string? logoUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(labelCode))
                throw new ArgumentException("LabelCode is required.");

            if (countryId.HasValue && countryId.Value == Guid.Empty)
                throw new ArgumentException("CountryId is invalid.");

            if (primaryLanguageId.HasValue && primaryLanguageId.Value == Guid.Empty)
                throw new ArgumentException("PrimaryLanguageId is invalid.");

            return new Label
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                LabelCode = labelCode.Trim(),
                Website = string.IsNullOrWhiteSpace(website) ? null : website.Trim(),
                InstagramUrl = string.IsNullOrWhiteSpace(instagramUrl) ? null : instagramUrl.Trim(),
                TikTokUrl = string.IsNullOrWhiteSpace(tikTokUrl) ? null : tikTokUrl.Trim(),
                YouTubeUrl = string.IsNullOrWhiteSpace(youTubeUrl) ? null : youTubeUrl.Trim(),
                FacebookUrl = string.IsNullOrWhiteSpace(facebookUrl) ? null : facebookUrl.Trim(),
                CountryId = countryId,
                PrimaryLanguageId = primaryLanguageId,
                LogoUrl = string.IsNullOrWhiteSpace(logoUrl) ? null : logoUrl.Trim(),
                ContactPerson = LabelContactPerson.Empty()
            };
        }

        public void SetContactPerson(string? fullName, string? email, string? phoneNumber, string? role, Guid? preferredLanguageId)
        {
            if (preferredLanguageId.HasValue && preferredLanguageId.Value == Guid.Empty)
                throw new ArgumentException("PreferredLanguageId is invalid.");

            ContactPerson = LabelContactPerson.Create(
                fullName,
                email,
                phoneNumber,
                role,
                preferredLanguageId);
        }

        public void UpdateDetails(
            string? name,
            string? labelCode,
            string? website,
            string? instagramUrl,
            string? tikTokUrl,
            string? youTubeUrl,
            string? facebookUrl,
            Guid? countryId,
            Guid? primaryLanguageId,
            string? logoUrl)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name.Trim();

            if (!string.IsNullOrWhiteSpace(labelCode))
                LabelCode = labelCode.Trim();

            Website = string.IsNullOrWhiteSpace(website) ? null : website.Trim();
            InstagramUrl = string.IsNullOrWhiteSpace(instagramUrl) ? null : instagramUrl.Trim();
            TikTokUrl = string.IsNullOrWhiteSpace(tikTokUrl) ? null : tikTokUrl.Trim();
            YouTubeUrl = string.IsNullOrWhiteSpace(youTubeUrl) ? null : youTubeUrl.Trim();
            FacebookUrl = string.IsNullOrWhiteSpace(facebookUrl) ? null : facebookUrl.Trim();

            if (countryId.HasValue)
            {
                if (countryId.Value == Guid.Empty)
                    throw new ArgumentException("CountryId is invalid.");

                CountryId = countryId.Value;
            }

            if (primaryLanguageId.HasValue)
            {
                if (primaryLanguageId.Value == Guid.Empty)
                    throw new ArgumentException("PrimaryLanguageId is invalid.");

                PrimaryLanguageId = primaryLanguageId.Value;
            }

            LogoUrl = string.IsNullOrWhiteSpace(logoUrl) ? null : logoUrl.Trim();
        }
    }
}