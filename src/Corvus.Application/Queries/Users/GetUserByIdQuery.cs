using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using MediatR;

namespace Corvus.Application.Queries.Users;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;