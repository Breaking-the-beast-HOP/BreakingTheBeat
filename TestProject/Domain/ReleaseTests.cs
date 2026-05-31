using MusicCatalogContext.Domain.Entities;

namespace BreakingTheBeat.Tests.Domain;

public class ReleaseTests
{
    private static Release NewValidRelease() =>
        Release.Create(
            title: "Test Release",
            version: null,
            ean: "0123456789012",
            releaseDate: null,
            genreId: Guid.NewGuid(),
            languageId: Guid.NewGuid());

    // [Fact]
    // Bruges når testen kun skal køres med ét fast scenarie.
    // Her tester vi et normalt "happy path" flow.
    [Fact]
    public void Create_WithValidData_CreatesRelease()
    {
        // Arrange + Act
        var release = NewValidRelease();

        // Assert
        Assert.NotEqual(Guid.Empty, release.Id);
        Assert.Equal("Test Release", release.Title);
        Assert.Empty(release.Volumes);
        Assert.Empty(release.Contributions);
    }

    // [Theory]
    // Bruges når samme test skal køres flere gange med forskellige input.
    //
    // [InlineData]
    // Sender forskellige testværdier ind i testen.
    // Her tester vi flere ugyldige titler uden at skrive flere næsten identiske tests.
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithMissingTitle_ThrowsException(string? title)
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var languageId = Guid.NewGuid();

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            Release.Create(title!, null, null, null, genreId, languageId));
    }

    // [Fact]
    // Bruges fordi vi kun tester ét specifikt scenarie.
    [Fact]
    public void Create_WithEmptyGenreId_ThrowsException()
    {
        // Arrange
        var languageId = Guid.NewGuid();

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            Release.Create("Test Release", null, null, null, Guid.Empty, languageId));
    }

    [Fact]
    public void Create_WithEmptyLanguageId_ThrowsException()
    {
        // Arrange
        var genreId = Guid.NewGuid();

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            Release.Create("Test Release", null, null, null, genreId, Guid.Empty));
    }

    [Fact]
    public void AddVolume_WithValidNumber_AddsVolume()
    {
        // Tester at et volume kan tilføjes til releasen.

        // Arrange
        var release = NewValidRelease();

        // Act
        var volume = release.AddVolume(1);

        // Assert
        Assert.Single(release.Volumes);
        Assert.Equal(1, volume.VolumeNumber);
    }

    // [Theory]
    // Samme test køres med flere ugyldige volume-numre.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddVolume_WithInvalidNumber_ThrowsException(int volumeNumber)
    {
        // Arrange
        var release = NewValidRelease();

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            release.AddVolume(volumeNumber));
    }

    [Fact]
    public void AddVolume_WithDuplicateNumber_ThrowsException()
    {
        // Tester at samme volumeNumber ikke kan tilføjes to gange.

        // Arrange
        var release = NewValidRelease();
        release.AddVolume(1);

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() =>
            release.AddVolume(1));
    }

    [Fact]
    public void AddContribution_WithValidData_AddsContribution()
    {
        // Tester at en contributor med en rolle kan tilføjes.

        // Arrange
        var release = NewValidRelease();
        var contributorId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        // Act
        release.AddContribution(contributorId, roleId);

        // Assert
        var contribution = Assert.Single(release.Contributions);

        Assert.Equal(contributorId, contribution.ContributorId);
        Assert.Equal(roleId, contribution.ContributorRoleId);
    }

    [Fact]
    public void AddContribution_WhenDuplicate_IsIgnored()
    {
        // Tester at dubletter ikke bliver tilføjet.

        // Arrange
        var release = NewValidRelease();
        var contributorId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        // Act
        release.AddContribution(contributorId, roleId);
        release.AddContribution(contributorId, roleId);

        // Assert
        Assert.Single(release.Contributions);
    }

    [Fact]
    public void AddContribution_WithEmptyContributorId_ThrowsException()
    {
        // Tester at contributorId skal være gyldigt.

        // Arrange
        var release = NewValidRelease();

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            release.AddContribution(Guid.Empty, Guid.NewGuid()));
    }

    [Fact]
    public void AddContribution_WithEmptyRoleId_ThrowsException()
    {
        // Tester at roleId skal være gyldigt.

        // Arrange
        var release = NewValidRelease();

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            release.AddContribution(Guid.NewGuid(), Guid.Empty));
    }

    [Fact]
    public void SubmitToQc_RaisesDomainEvent()
    {
        // Tester at der oprettes et domain event når releasen sendes til QC.

        // Arrange
        var release = NewValidRelease();

        // Act
        release.SubmitToQc();

        // Assert
        Assert.Single(release.DomainEvents);
    }

    [Fact]
    public void UpdateDetails_WithNewLanguageId_UpdatesLanguageId()
    {
        // Tester at LanguageId bliver opdateret korrekt.

        // Arrange
        var release = NewValidRelease();
        var newLanguageId = Guid.NewGuid();

        // Act
        release.UpdateDetails(
            title: null,
            version: null,
            ean: null,
            genreId: null,
            languageId: newLanguageId);

        // Assert
        Assert.Equal(newLanguageId, release.LanguageId);
    }
}