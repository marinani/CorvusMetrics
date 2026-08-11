using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Commands.Users;

public sealed record DeleteUserCommand(Guid Id) : IRequest<Result>;