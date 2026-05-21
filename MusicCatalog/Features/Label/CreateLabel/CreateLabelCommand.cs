using MediatR;

namespace MusicCatalog.Features.Label.Create
{
    public record CreateLabelCommand(
        string Name,
        string LabelCode,
        string? Website,
        string? InstagramUrl,
        string? TikTokUrl,
        string? YouTubeUrl,
        string? FacebookUrl,
        Guid? CountryId,
        Guid? PrimaryLanguageId,
        string? LogoUrl,
        string? ContactFullName,
        string? ContactEmail,
        string? ContactPhoneNumber,
        string? ContactRole,
        Guid? ContactPreferredLanguageId
    ) : IRequest<Guid>;
}