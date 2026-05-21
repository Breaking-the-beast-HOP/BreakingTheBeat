using MediatR;

namespace MusicCatalogContext.Features.Contributor.Delete
{
    public class DeleteContributorHandler : IRequestHandler<DeleteContributorCommand, bool>
    {
        private readonly IContributorRepository _repository;

        public DeleteContributorHandler(IContributorRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteContributorCommand request, CancellationToken cancellationToken)
        {
            var contributor = await _repository.GetByIdAsync(request.Id);

            if (contributor is null)
                return false;

            await _repository.DeleteAsync(contributor.Id);

            return true;
        }
    }
}