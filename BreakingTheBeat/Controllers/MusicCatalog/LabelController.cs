using Gateway.Contracts.Music.Label;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Features.Label.Create;

namespace Gateway.Controllers.MusicCatalog
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LabelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Content Creator,Quality controller")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateLabelRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateLabelCommand(
                request.Name,
                request.LabelCode,
                request.Website,
                request.InstagramUrl,
                request.TikTokUrl,
                request.YouTubeUrl,
                request.FacebookUrl,
                request.CountryId,
                request.PrimaryLanguageId,
                request.LogoUrl,
                request.ContactPerson.FullName,
                request.ContactPerson.Email,
                request.ContactPerson.PhoneNumber,
                request.ContactPerson.Role,
                request.ContactPerson.PreferredLanguageId
            );

            var labelId = await _mediator.Send(command, cancellationToken);

            return Ok(labelId);
        }
    }
}