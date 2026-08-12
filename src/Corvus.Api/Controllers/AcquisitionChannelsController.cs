using Corvus.Application.Commands.AcquisitionChannels;
using Corvus.Application.Queries.AcquisitionChannels;
using Corvus.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Corvus.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/acquisition-channels")]
public sealed class AcquisitionChannelsController : ControllerBase
{
    private readonly ISender _sender;

    public AcquisitionChannelsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        CreateAcquisitionChannelCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new { acquisitionChannelId = result.Value })
            : BadRequest(new { result.Error!.Code, result.Error.Message });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? name = null,
        [FromQuery] EntityStatusFilter status = EntityStatusFilter.All,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAcquisitionChannelsQuery(pageNumber, pageSize, name, status), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAcquisitionChannelByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { result.Error!.Code, result.Error.Message });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAcquisitionChannelCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command with { Id = id }, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : NotFound(new { result.Error!.Code, result.Error.Message });
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteAcquisitionChannelCommand(id), cancellationToken);

        return result.IsSuccess
            ? Ok()
            : NotFound(new { result.Error!.Code, result.Error.Message });
    }
}