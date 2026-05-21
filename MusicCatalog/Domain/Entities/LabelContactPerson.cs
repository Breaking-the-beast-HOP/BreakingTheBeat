namespace MusicCatalogContext.Domain.Entities
{

    public class LabelContactPerson
    {
        public string? FullName { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? Role { get; private set; }

        public Guid? PreferredLanguageId { get; private set; }
        public Language? PreferredLanguage { get; private set; }

        private LabelContactPerson() { }

        private LabelContactPerson(
            string? fullName,
            string? email,
            string? phoneNumber,
            string? role,
            Guid? preferredLanguageId)
        {
            FullName = string.IsNullOrWhiteSpace(fullName) ? null : fullName.Trim();
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
            Role = string.IsNullOrWhiteSpace(role) ? null : role.Trim();
            PreferredLanguageId = preferredLanguageId;
        }

        public static LabelContactPerson Create(
            string? fullName,
            string? email,
            string? phoneNumber,
            string? role,
            Guid? preferredLanguageId)
        {
            if (preferredLanguageId.HasValue && preferredLanguageId.Value == Guid.Empty)
                throw new ArgumentException("PreferredLanguageId is invalid.");

            return new LabelContactPerson(
                fullName,
                email,
                phoneNumber,
                role,
                preferredLanguageId);
        }

        public static LabelContactPerson Empty()
        {
            return new LabelContactPerson(
                null,
                null,
                null,
                null,
                null);
        }
    }

}
