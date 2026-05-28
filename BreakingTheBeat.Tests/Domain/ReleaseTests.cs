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

    [Fact]
    public void Create_WithValidArguments_ReturnsRelease()
    {
        var release = NewValidRelease();

        Assert.NotEqual(Guid.Empty, release.Id);
        Assert.Equal("Test Release", release.Title);
        Assert.Empty(release.Volumes);
        Assert.Empty(release.Contributions);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithMissingTitle_Throws(string? title)
    {
        Assert.Throws<ArgumentException>(() =>
            Release.Create(title!, null, null, null, Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public void Create_WithEmptyGenreId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            Release.Create("title", null, null, null, Guid.Empty, Guid.NewGuid()));
    }

    [Fact]
    public void Create_WithEmptyLanguageId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            Release.Create("title", null, null, null, Guid.NewGuid(), Guid.Empty));
    }

    [Fact]
    public void AddVolume_WithPositiveNumber_IsAdded()
    {
        var release = NewValidRelease();

        var volume = release.AddVolume(1);

        Assert.NotNull(volume);
        Assert.Single(release.Volumes);
        Assert.Equal(1, volume.VolumeNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddVolume_WithNonPositiveNumber_Throws(int volumeNumber)
    {
        var release = NewValidRelease();

        Assert.Throws<ArgumentException>(() => release.AddVolume(volumeNumber));
    }

    [Fact]
    public void AddVolume_DuplicateNumber_Throws()
    {
        var release = NewValidRelease();
        release.AddVolume(1);

        Assert.Throws<InvalidOperationException>(() => release.AddVolume(1));
    }

    [Fact]
    public void AddContribution_NewContribution_IsAdded()
    {
        var release = NewValidRelease();
        var contributorId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        release.AddContribution(contributorId, roleId);

        var added = Assert.Single(release.Contributions);
        Assert.Equal(contributorId, added.ContributorId);
        Assert.Equal(roleId, added.ContributorRoleId);
    }

    [Fact]
    public void AddContribution_Duplicate_IsIgnored()
    {
        var release = NewValidRelease();
        var contributorId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        release.AddContribution(contributorId, roleId);
        release.AddContribution(contributorId, roleId);

        Assert.Single(release.Contributions);
    }

    [Fact]
    public void AddContribution_WithEmptyContributorId_Throws()
    {
        var release = NewValidRelease();

        Assert.Throws<ArgumentException>(() =>
            release.AddContribution(Guid.Empty, Guid.NewGuid()));
    }

    [Fact]
    public void AddContribution_WithEmptyRoleId_Throws()
    {
        var release = NewValidRelease();

        Assert.Throws<ArgumentException>(() =>
            release.AddContribution(Guid.NewGuid(), Guid.Empty));
    }

    [Fact]
    public void SubmitToQc_RaisesDomainEvent()
    {
        var release = NewValidRelease();

        release.SubmitToQc();

        Assert.Single(release.DomainEvents);
    }

    [Fact(Skip = "Documents bug in Release.UpdateDetails — see ARCHITECTURE_REVIEW.md #2. " +
                 "The 'languageId = languageId.Value;' line reassigns the parameter to itself " +
                 "instead of assigning to the LanguageId property. Un-skip when fixed.")]
    public void UpdateDetails_NewLanguageId_IsPersisted()
    {
        var release = NewValidRelease();
        var newLanguageId = Guid.NewGuid();

        release.UpdateDetails(
            title: null,
            version: null,
            ean: null,
            genreId: null,
            languageId: newLanguageId);

        Assert.Equal(newLanguageId, release.LanguageId);
    }
}
