//using FluentAssertions;
//using PodcastCatalog.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PodcastCatalogTest.Models
//{
//    [TestClass]
//    public class ContributionTests
//    {
//        [TestMethod]
//        public void Constructor_ShouldSetValues()
//        {
//            var contributor = new Contributor(Guid.NewGuid(), "John", "Desc");
//            var role = new Role(Guid.NewGuid(), "Host");

//            var contribution = new Contribution(Guid.NewGuid(), contributor, role, true);

//            contribution.Contributor.Should().Be(contributor);
//            contribution.Role.Should().Be(role);
//            contribution.HasRights.Should().BeTrue();
//        }

//        [TestMethod]
//        public void Constructor_ShouldThrow_WhenContributorIsNull()
//        {
//            var role = new Role(Guid.NewGuid(), "Host");

//            Action act = () => new Contribution(Guid.NewGuid(), null, role, true);

//            act.Should().Throw<ArgumentNullException>();
//        }

//        [TestMethod]
//        public void ChangeRole_ShouldUpdateRole()
//        {
//            var contributor = new Contributor(Guid.NewGuid(), "John", "Desc");
//            var role1 = new Role(Guid.NewGuid(), "Host");
//            var role2 = new Role(Guid.NewGuid(), "Guest");

//            var contribution = new Contribution(Guid.NewGuid(), contributor, role1, true);

//            contribution.ChangeRole(role2);

//            contribution.Role.Should().Be(role2);
//        }
//    }
//}
