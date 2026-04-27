namespace MusicCatalog.Domain.Entities
{
    public class Contribution
    {
        public Guid Id { get; private set; }

        public Guid ContributorId { get; private set; }
        public Contributor Contributor { get; private set; }

        public Guid ContributorRoleId { get; private set; }
        public ContributorRole Role { get; private set; }

        public Guid? TrackId { get; private set; }
        public Track? Track { get; private set; }

        public Guid? ReleaseId { get; private set; }
        public Release? Release { get; private set; }

        private Contribution() { }

        private Contribution(Guid contributorId, Guid contributorRoleId, Guid? trackId, Guid? releaseId)
        {
            Id = Guid.NewGuid();

            ContributorId = contributorId;
            ContributorRoleId = contributorRoleId;
            TrackId = trackId;
            ReleaseId = releaseId;
        }

        public static Contribution ForTrack(Guid trackId, Guid contributorId, Guid roleId)
        {
            return new Contribution(contributorId, roleId, trackId, null);
        }

        public static Contribution ForRelease(Guid releaseId, Guid contributorId, Guid roleId)
        {
            return new Contribution(contributorId, roleId, null, releaseId);
        }
    }
}
