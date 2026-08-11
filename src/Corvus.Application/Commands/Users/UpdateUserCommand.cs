using Corvus.Application.Common.Results;
using Corvus.Domain.Enums;
using MediatR;

namespace Corvus.Application.Commands.Users;

public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    UserRole Role) : IRequest<Result>;