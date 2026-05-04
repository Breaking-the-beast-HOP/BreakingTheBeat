using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Languages.Create
{
    public class CreateLanguageHandler : IRequestHandler<CreateNameCommand, LanguageDTO>
    {
        private readonly ICrudRepository<Language> _languageRepository;
        private readonly IValidator<CreateNameCommand> _validator;

        public CreateLanguageHandler(ICrudRepository<Language> languageRepository, IValidator<CreateNameCommand> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public async Task<LanguageDTO> Handle(CreateNameCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var language = new Language(Guid.NewGuid(), request.Name, request.Code);

            await _languageRepository.AddAsync(language);

            return new LanguageDTO
            {
                Id = language.Id,
                Name = language.Name,
                ISO = language.ISO
            };
        }
    }
}