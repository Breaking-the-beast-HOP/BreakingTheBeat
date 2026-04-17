using FluentAssertions;
using PodcastCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalogTest.Models
{
    [TestClass]
    public class TagTests
    {
        [TestMethod]
        public void Constructor_ShouldSetName()
        {
            var tag = new Tag(Guid.NewGuid(), "Tech");

            tag.Name.Should().Be("Tech");
        }

        [TestMethod]
        public void Constructor_ShouldThrow_WhenNameIsEmpty()
        {
            Action act = () => new Tag(Guid.NewGuid(), "");

            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Rename_ShouldUpdateName()
        {
            var tag = new Tag(Guid.NewGuid(), "Old");

            tag.Rename("New");

            tag.Name.Should().Be("New");
        }
    }

}
