using MediatR;
using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Infrastructure.Repositories;

namespace PodcastCatalog.Features.Contributors.Create;

public class CreatePodcastContributorHandler : IRequestHandler<CreatePodcastContributorCommand, Guid>
{
    private readonly IContributorRepository _repository;

    public CreatePodcastContributorHandler(IContributorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CreatePodcastContributorCommand request,
        CancellationToken cancellationToken)
    {
        var contributor = Contributor.Create(
            request.DisplayName,
            request.FullName,
            request.Title,
            request.Biography,
            request.Expertise,
            request.ProfileUrl
        );

        await _repository.AddAsync(contributor);

        return contributor.Id;
    }
}