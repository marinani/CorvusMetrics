using Corvus.Application.Commands.Tenants;
using Corvus.Application.Queries.Tenants;
using Corvus.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Corvus.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/tenants")]
public sealed class TenantsController : ControllerBase
{
    private readonly ISender _sender;

    public TenantsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        CreateTenantCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new { tenantId = result.Value })
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
        var result = await _sender.Send(new GetTenantsQuery(pageNumber, pageSize, name, status), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetActiveTenantsQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTenantByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { result.Error!.Code, result.Error.Message });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTenantCommand command,
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
        var result = await _sender.Send(new DeleteTenantCommand(id), cancellationToken);

        return result.IsSuccess
            ? Ok()
            : NotFound(new { result.Error!.Code, result.Error.Message });
    }
}