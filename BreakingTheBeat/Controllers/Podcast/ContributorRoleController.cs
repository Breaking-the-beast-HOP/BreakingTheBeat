using Gateway.Contracts.Podcast.ContributionRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PodcastCatalog.Features.ContributorRoles.GetAll;

namespace Gateway.Controllers.Podcast
{
    [Route("api/podcasts/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin, Content Creator, Quality controller")]
    [ApiController]
    public class ContributorRoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ContributorRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetContributorRoles(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllContributionRolesQuery(), cancellationToken);

            var responses = result.Select(r => new ContributionRoleResponse
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList();
            return Ok(responses);
        }
    }
}
