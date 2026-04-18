using Gateway.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Genre;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetLGenres")]
        public async Task<ActionResult<List<GenreResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetGenresQuery(), cancellationToken);
            return Ok(result);
        }
    }
    

}
