using Gateway.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Language.GetLanguages;

namespace Gateway.Controllers.Music
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LanguageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetLanguages")]
        public async Task<ActionResult<List<LanguageResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetLanguagesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}