using Gateway.Contracts.Podcast.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodcastCatalog.Features.Categories.GetAll;

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
        public async Task<ActionResult<List<CategoryResponse>>> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
            var categories = result.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
            return Ok(categories);
        }
    }
}
