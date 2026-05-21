using Gateway.Contracts.Music.Genre;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogContext.Features.Genre;

namespace Gateway.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetGenres")]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
        public async Task<ActionResult<List<GenreResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetGenresQuery(), cancellationToken);
            return Ok(result);
        }
    }


}
