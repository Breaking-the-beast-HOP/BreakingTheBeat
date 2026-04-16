using Gateway.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Language.GetLanguages;
using MusicCatalog.Features.Languages.GetLanguages;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
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