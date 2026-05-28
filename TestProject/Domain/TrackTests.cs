using MusicCatalogContext.Domain.Entities;

namespace BreakingTheBeat.Tests.Domain;

public class TrackTests
{
    private static Track NewValidTrack() =>
        Track.Create(
            title: "Test Track",
            version: null,
            isrc: "USRC17607839",
            genreId: Guid.NewGuid(),
            audioLanguageId: Guid.NewGuid(),
            metadataLanguageId: Guid.NewGuid());

    [Fact]
    public void Create_WithValidArguments_ReturnsTrack()
    {
        var track = NewValidTrack();

        Assert.NotEqual(Guid.Empty, track.Id);
        Assert.Equal("Test Track", track.Title);
        Assert.Equal(TimeSpan.Zero, track.Duration);
        Assert.Empty(track.Contributions);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithMissingTitle_Throws(string? title)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Track.Create(title!, null, "isrc", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));

        Assert.Contains("Title", ex.Message);
    }

    [Fact]
    public void Create_WithEmptyGenreId_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Track.Create("title", null, "isrc", Guid.Empty, Guid.NewGuid(), Guid.NewGuid()));

        Assert.Contains("GenreId", ex.Message);
    }

    [Fact]
    public void Create_WithEmptyMetadataLanguageId_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Track.Create("title", null, "isrc", Guid.NewGuid(), Guid.NewGuid(), Guid.Empty));

        Assert.Contains("MetadataLanguageId", ex.Message);
    }

    [Fact]
    public void Create_WithEmptyAudioLanguageId_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            Track.Create("title", null, "isrc", Guid.NewGuid(), Guid.Empty, Guid.NewGuid()));

        Assert.Contains("AudioLanguageId", ex.Message);
    }

    [Fact]
    public void AddContribution_NewContribution_IsAdded()
    {
        var track = NewValidTrack();
        var contributorId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        track.AddContribution(contributorId, roleId);

        Assert.Single(track.Contributions);
        var added = Assert.Single(track.Contributions);
        Assert.Equal(contributorId, added.ContributorId);
        Assert.Equal(roleId, added.ContributorRoleId);
    }

    [Fact]
    public void AddContribution_DuplicateContributorAndRole_IsIgnored()
    {
        var track = NewValidTrack();
        var contributorId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        track.AddContribution(contributorId, roleId);
        track.AddContribution(contributorId, roleId);

        Assert.Single(track.Contributions);
    }

    [Fact]
    public void AddContribution_SameContributorDifferentRole_BothAdded()
    {
        var track = NewValidTrack();
        var contributorId = Guid.NewGuid();

        track.AddContribution(contributorId, Guid.NewGuid());
        track.AddContribution(contributorId, Guid.NewGuid());

        Assert.Equal(2, track.Contributions.Count);
    }
}
