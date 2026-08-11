using Corvus.Application.Common.Results;
using Corvus.Domain.Enums;
using MediatR;

namespace Corvus.Application.Commands.Users;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role) : IRequest<Result<Guid>>;