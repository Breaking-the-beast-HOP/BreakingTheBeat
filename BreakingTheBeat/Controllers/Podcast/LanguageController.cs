using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PodcastCatalog.Features.Languages.GetAll;
using Gateway.Contracts.Podcast;

namespace Gateway.Controllers.Podcast
{
    [Route("api/podcasts/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LanguageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<LanguageDetailsResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllLanguagesQuery(), cancellationToken);
            var languages = result.Select(l => new LanguageDetailsResponse
            {
                Id = l.Id,
                Name = l.Name,
                ISO = l.ISO // Mapper direkte fra LanguageDTO.ISO
            }).ToList();
            return Ok(languages);
        }
    }
}
