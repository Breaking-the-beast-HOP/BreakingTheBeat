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

    // [Fact]
    // Bruges når vi tester ét konkret scenarie.
    // Her tester vi at et Track oprettes korrekt med gyldige data.
    [Fact]
    public void Create_WithValidArguments_ReturnsTrack()
    {
        // Arrange + Act
        var track = NewValidTrack();

        // Assert
        Assert.NotEqual(Guid.Empty, track.Id);
        Assert.Equal("Test Track", track.Title);
        Assert.Equal(TimeSpan.Zero, track.Duration);
        Assert.Empty(track.Contributions);
    }

    // [Theory]
    // Bruges når samme test skal køres flere gange med forskellige input.
    //
    // [InlineData]
    // Sender forskellige ugyldige titler ind i testen.
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithMissingTitle_ThrowsException(string? title)
    {
        // Arrange + Act
        var ex = Assert.Throws<ArgumentException>(() =>
            Track.Create(
                title!,
                null,
                "isrc",
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()));

        // Assert
        Assert.Contains("Title", ex.Message);
    }

    // [Fact]
    // Tester ét specifikt scenarie:
    // GenreId må ikke være tomt.
    [Fact]
    public void Create_WithEmptyGenreId_ThrowsException()
    {
        // Arrange + Act
        var ex = Assert.Throws<ArgumentException>(() =>
            Track.Create(
                "title",
                null,
                "isrc",
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid()));

        // Assert
        Assert.Contains("GenreId", ex.Message);
    }

    [Fact]
    public void Create_WithEmptyMetadataLanguageId_ThrowsException()
    {
        // Tester at MetadataLanguageId skal være gyldigt.

        // Arrange + Act
        var ex = Assert.Throws<ArgumentException>(() =>
            Track.Create(
                "title",
                null,
                "isrc",
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty));

        // Assert
        Assert.Contains("MetadataLanguageId", ex.Message);
    }

    [Fact]
    public void Create_WithEmptyAudioLanguageId_ThrowsException()
    {
        // Tester at AudioLanguageId skal være gyldigt.

        // Arrange + Act
        var ex = Assert.Throws<ArgumentException>(() =>
            Track.Create(
                "title",
                null,
                "isrc",
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid()));

        // Assert
        Assert.Contains("AudioLanguageId", ex.Message);
    }

    [Fact]
    public void AddContribution_NewContribution_IsAdded()
    {
        // Tester at en contributor med en rolle bliver tilføjet til tracket.

        // Arrange
        var track = NewValidTrack();
        var contributorId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        // Act
        track.AddContribution(contributorId, roleId);

        // Assert
        var added = Assert.Single(track.Contributions);

        Assert.Equal(contributorId, added.ContributorId);
        Assert.Equal(roleId, added.ContributorRoleId);
    }

    [Fact]
    public void AddContribution_DuplicateContributorAndRole_IsIgnored()
    {
        // Tester at samme contributor + rolle ikke tilføjes flere gange.

        // Arrange
        var track = NewValidTrack();
        var contributorId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        // Act
        track.AddContribution(contributorId, roleId);
        track.AddContribution(contributorId, roleId);

        // Assert
        Assert.Single(track.Contributions);
    }

    [Fact]
    public void AddContribution_SameContributorDifferentRole_BothAdded()
    {
        // Tester at samme contributor godt må have flere forskellige roller.

        // Arrange
        var track = NewValidTrack();
        var contributorId = Guid.NewGuid();

        // Act
        track.AddContribution(contributorId, Guid.NewGuid());
        track.AddContribution(contributorId, Guid.NewGuid());

        // Assert
        Assert.Equal(2, track.Contributions.Count);
    }
}