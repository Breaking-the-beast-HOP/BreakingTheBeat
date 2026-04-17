using FluentAssertions;
using PodcastCatalog.Models;

namespace PodcastCatalogTest.Models
{
    [TestClass]
    public class EpisodeTests
    {
        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var episode = CreateEpisode();

            episode.Title.Should().Be("Test");
            episode.Description.Should().Be("Desc");
        }

        [TestMethod]
        public void Constructor_ShouldThrow_WhenTitleIsEmpty()
        {
            Action act = () => new Episode(
                Guid.NewGuid(),
                "",
                "Desc",
                TimeSpan.FromMinutes(10),
                DateOnly.FromDateTime(DateTime.Today));

            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void AddTag_ShouldAddTag()
        {
            var episode = CreateEpisode();
            var tag = new Tag(Guid.NewGuid(), "Tech");

            episode.AddTag(tag);

            episode.Tags.Should().ContainSingle();
        }

        [TestMethod]
        public void AddTag_ShouldNotAddDuplicate()
        {
            var episode = CreateEpisode();
            var tag = new Tag(Guid.NewGuid(), "Tech");

            episode.AddTag(tag);
            episode.AddTag(tag);

            episode.Tags.Count.Should().Be(1);
        }

        [TestMethod]
        public void AddTag_ShouldThrow_WhenNull()
        {
            var episode = CreateEpisode();

            Action act = () => episode.AddTag(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void AddContribution_ShouldAddContribution()
        {
            var episode = CreateEpisode();
            var contributor = CreateContributor();
            var role = CreateRole();

            episode.AddContribution(contributor, role, true);

            episode.Contributions.Should().ContainSingle();
        }

        [TestMethod]
        public void AddContribution_ShouldNotAddDuplicate()
        {
            var episode = CreateEpisode();
            var contributor = CreateContributor();
            var role = CreateRole();

            episode.AddContribution(contributor, role, true);
            episode.AddContribution(contributor, role, true);

            episode.Contributions.Count.Should().Be(1);
        }

        [TestMethod]
        public void AddContribution_ShouldAllowSameContributorWithDifferentRole()
        {
            var episode = CreateEpisode();
            var contributor = CreateContributor();

            var role1 = new Role(Guid.NewGuid(), "Host");
            var role2 = new Role(Guid.NewGuid(), "Guest");

            episode.AddContribution(contributor, role1, true);
            episode.AddContribution(contributor, role2, true);

            episode.Contributions.Count.Should().Be(2);
        }

        [TestMethod]
        public void RemoveContribution_ShouldRemoveCorrectOne()
        {
            var episode = CreateEpisode();
            var contributor = CreateContributor();
            var role = CreateRole();

            episode.AddContribution(contributor, role, true);

            episode.RemoveContribution(contributor.Id, role.Id);

            episode.Contributions.Should().BeEmpty();
        }

        // 🔹 Helpers
        private Episode CreateEpisode()
        {
            return new Episode(
                Guid.NewGuid(),
                "Test",
                "Desc",
                TimeSpan.FromMinutes(10),
                DateOnly.FromDateTime(DateTime.Today));
        }

        private Contributor CreateContributor()
        {
            return new Contributor(Guid.NewGuid(), "John", "Guest");
        }

        private Role CreateRole()
        {
            return new Role(Guid.NewGuid(), "Host");
        }
    }
}
