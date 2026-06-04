using Gateway.Contracts.Music.Country;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogContext.Features.Countries.Get;

namespace Gateway.Controllers.MusicCatalog
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetCountries")]
        [Authorize(Roles = "Admin,Content creator,Quality controller")]
        public async Task<ActionResult<List<CountryResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllCountriesQuery(), cancellationToken);

            var response = result.Select(x => new CountryResponse
            {
                Id = x.Id,
                Name = x.Name,
                ISO = x.ISO
            }).ToList();

            return Ok(response);
        }
    }
}
