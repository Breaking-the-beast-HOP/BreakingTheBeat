using MediatR;
using MusicCatalogContext.Infrastructure;

namespace MusicCatalog.Features.Label.Create
{
    public class CreateLabelHandler : IRequestHandler<CreateLabelCommand, Guid>
    {

        private readonly ILabelRepository _repository;

        public CreateLabelHandler (ILabelRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateLabelCommand request, CancellationToken cancellationToken)
        {
            var label = MusicCatalogContext.Domain.Entities.Label.Create(
                request.Name,
                request.LabelCode,
                request.Website,
                request.InstagramUrl,
                request.TikTokUrl,
                request.YouTubeUrl,
                request.FacebookUrl,
                request.CountryId,
                request.PrimaryLanguageId,
                request.LogoUrl
            );

            label.SetContactPerson(
                request.ContactFullName,
                request.ContactEmail,
                request.ContactPhoneNumber,
                request.ContactRole,
                request.ContactPreferredLanguageId
            );

            _repository.AddAsync(label);

            return label.Id;
        }
    }
}