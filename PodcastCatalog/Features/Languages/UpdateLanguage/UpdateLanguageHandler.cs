using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Languages.UpdateLanguage
{
    public class UpdateLanguageHandler : IRequestHandler<UpdateLanguageCommand, LanguageDTO>
    {
        private readonly ICrudRepository<Language> _languageRepository;
        private readonly IValidator<UpdateLanguageCommand> _validator;

        public UpdateLanguageHandler(ICrudRepository<Language> languageRepository, IValidator<UpdateLanguageCommand> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public async Task<LanguageDTO> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var language = await _languageRepository.GetByIdAsync(request.Id);
            if (language == null)
            {
                throw new KeyNotFoundException($"Language with ID {request.Id} not found.");
            }

            language.Update(request.Name, request.ISO);
            await _languageRepository.UpdateAsync(language);

            return new LanguageDTO
            {
                Id = language.Id,
                Name = language.Name,
                ISO = language.ISO
            };
        }
    }
}
