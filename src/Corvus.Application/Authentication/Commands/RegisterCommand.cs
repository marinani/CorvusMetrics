using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Authentication.Commands;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<Result<Guid>>;