using Corvus.Application.Commands.Users;
using Corvus.Application.Queries.Users;
using Corvus.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Corvus.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new { userId = result.Value })
            : BadRequest(new { result.Error!.Code, result.Error.Message });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? firstName = null,
        [FromQuery] string? lastName = null,
        [FromQuery] UserRole? role = null,
        [FromQuery] UserStatusFilter status = UserStatusFilter.All,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetUsersQuery(pageNumber, pageSize, firstName, lastName, role, status), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { result.Error!.Code, result.Error.Message });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserCommand command,
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
        var result = await _sender.Send(new DeleteUserCommand(id), cancellationToken);

        return result.IsSuccess
            ? Ok()
            : NotFound(new { result.Error!.Code, result.Error.Message });
    }
}
