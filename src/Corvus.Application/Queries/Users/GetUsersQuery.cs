using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Common;
using Corvus.Domain.Enums;
using MediatR;

namespace Corvus.Application.Queries.Users;

public sealed record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? FirstName = null,
    string? LastName = null,
    UserRole? Role = null,
    EntityStatusFilter Status = EntityStatusFilter.All) : IRequest<Result<PagedResult<UserDto>>>;
