using Gateway.Contracts.Music.Language;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogContext.Features.Language.GetLanguages;

namespace Gateway.Controllers.Music
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LanguageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetLanguages")]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
        public async Task<ActionResult<List<LanguageResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetLanguagesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}