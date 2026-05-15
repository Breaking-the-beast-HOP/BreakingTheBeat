using Gateway.Contracts.MusicQC;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicQualityControl.Features.SubmitReleaseToQc;

namespace Gateway.Controllers.MusicQC;

[Route("api/qc/releases")]
[ApiController]
public class QcReleaseInboxController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public QcReleaseInboxController(IMediator mediator,IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveReleaseSubmittedToQc(
        [FromHeader(Name = "X-Worker-Key")] string workerKey,
        [FromBody] ReleaseSubmittedToQcDto dto,
        CancellationToken cancellationToken)
    {
        if (workerKey != _configuration["WorkerAuth:Key"])
            return Unauthorized();

        var command = new SubmitReleaseToQcCommand(
            dto.ReleaseId,
            dto.Title,
            dto.Version,
            dto.EAN
        );

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}