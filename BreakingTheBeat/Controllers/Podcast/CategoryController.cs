using Gateway.Contracts.Podcast.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PodcastCatalog.Features.Categories.Get;

namespace Gateway.Controllers.Podcast
{
    [Route("api/podcasts/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriesDetailsResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
            var categories = result.Select(c => new CategoriesDetailsResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
            return Ok(categories);
        }
    }
}
