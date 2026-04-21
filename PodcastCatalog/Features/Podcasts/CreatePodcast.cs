using FluentValidation;
using PodcastCatalog.Endpoints;
using PodcastCatalog.Shared.Infrastructure.Repositories;
using PodcastCatalog.Shared.Models;

namespace PodcastCatalog.Features.Podcasts;

public static class CreatePodcast
{
    // Request-model (DTO) uden Author
    public record Request(string Title, string Description);

    // Response-model (DTO)
    public record Response(Guid Id, string Title, string Description);

    // Validering med FluentValidation
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        }
    }

    // Endpoint-registrering
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("podcast", Handler)
               .WithTags("Podcasts")
               .Produces<Response>(StatusCodes.Status201Created)
               .ProducesValidationProblem(StatusCodes.Status400BadRequest);
        }
    }

    // Handler-logik
    public static async Task<IResult> Handler(
        Request request,
        IPodcastRepository repository,
        CancellationToken ct)
    {
        // Valider input
        var validator = new Validator();
        var validationResult = await validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        // Opret Podcast-domæneobjekt
        var podcast = new Podcast(
            id: Guid.NewGuid(),
            title: request.Title,
            description: request.Description);

        // Gem via repository
        await repository.AddAsync(podcast, ct);

        // Returner respons
        var response = new Response(
            Id: podcast.Id,
            Title: podcast.Title,
            Description: podcast.Description);

        return Results.Created($"podcast/{response.Id}", response);
    }
}