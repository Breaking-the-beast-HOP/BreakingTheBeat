using Gateway.Contracts.MusicQC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicQualityControl.Features.DeleteReleaseFromQc;
using MusicQualityControl.Features.GetQcQueue;
using MusicQualityControl.Features.SubmitReleaseToQc;

namespace Gateway.Controllers.MusicQC;

[Route("api/qc/releases")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
public class QcReleaseController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public QcReleaseController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ReceiveReleaseSubmittedToQc([FromHeader(Name = "X-Worker-Key")] string workerKey,[FromBody] ReleaseSubmittedToQcDto dto,CancellationToken cancellationToken)
    {

       /*var expectedKey = _configuration["WorkerAuth:Key"];  //slet mig*/
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

    [HttpGet]
    [Authorize(Roles = "Admin,Quality controller")]
    public async Task<IActionResult> GetQcQueue(CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetQcQueueQuery(), cancellationToken);

        var result = items.Select(x => new ReleaseQcQueueItemResponse(
            x.Id,
            x.ReleaseId,
            x.Title,
            x.Version,
            x.EAN,
            x.ReceivedAtUtc
        ));

        return Ok(result);
    }

    [HttpDelete("{releaseId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> WithdrawReleaseFromQc(Guid releaseId, [FromHeader(Name = "X-Worker-Key")] string workerKey,
    CancellationToken cancellationToken)
    {
        if (workerKey != _configuration["WorkerAuth:Key"])
            return Unauthorized();

        var deleted = await _mediator.Send(new WithdrawReleaseFromQcCommand(releaseId), cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

}
