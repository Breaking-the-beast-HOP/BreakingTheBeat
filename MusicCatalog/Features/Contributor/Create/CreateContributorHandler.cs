using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MusicCatalog.Domain.Entities;
using MusicCatalog.Features.Track;
using MusicCatalog.Features.Track.Create;

namespace MusicCatalog.Features.Contributor.Create
{
    public class CreateContributorHandler : IRequestHandler<CreateContributorCommand, Guid>
    {
        private readonly IContributorRepository _repository;

        public CreateContributorHandler(IContributorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateContributorCommand request, CancellationToken cancellationToken)
        {

            var contributor = MusicCatalog.Domain.Entities.Contributor.Create(
                request.DisplayName,
                request.LegalName,
                request.IPI,
                request.ISNI
            );

            await _repository.AddAsync(contributor);

            return contributor.Id;
        }
    }
}

