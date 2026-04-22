// Feature/Podcast/Create/CreatePodcastHandler.cs
using FluentValidation;
using MediatR;
using PodcastCatalog.Domain.Entities;
using SharedKernel.Interfaces.Base;

namespace PodcastCatalog.Features.Podcasts.Create;

public class CreatePodcastHandler : IRequestHandler<CreatePodcastCommand, PodcastDTO>
{
    private readonly ICreateRepository<Podcast> _podcastRepository;
    private readonly IValidator<CreatePodcastCommand> _validator;

    public CreatePodcastHandler(ICreateRepository<Podcast> podcastRepository,
                                IValidator<CreatePodcastCommand> validator)
    {
        _podcastRepository = podcastRepository;
        _validator = validator;

    }

    public async Task<PodcastDTO> Handle(CreatePodcastCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var podcast = new Podcast(
            id: Guid.NewGuid(),
            title: request.Title,
            description: request.Description);

        await _podcastRepository.AddAsync(podcast);

        return new PodcastDTO
        {
            Id = podcast.Id,
            Title = podcast.Title,
            Description = podcast.Description
        };
    }
}