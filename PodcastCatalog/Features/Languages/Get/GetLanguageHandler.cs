using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Features.Languages.Get
{
    public class GetLanguageHandler : IRequestHandler<GetLanguageQuery, LanguageDTO>
    {
        private readonly ICrudRepository<Language> _languageRepository;
        private readonly IValidator<GetLanguageQuery> _validator;

        public GetLanguageHandler(ICrudRepository<Language> languageRepository, IValidator<GetLanguageQuery> validator)
        {
            _languageRepository = languageRepository;
            _validator = validator;
        }

        public async Task<LanguageDTO> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var language = await _languageRepository.GetByIdAsync(request.Id);
            if (language == null)
            {
                return null;
            }

            return new LanguageDTO
            {
                Id = language.Id,
                Name = language.Name,
                IsoCode = language.ISO
            };
        }
    }
}