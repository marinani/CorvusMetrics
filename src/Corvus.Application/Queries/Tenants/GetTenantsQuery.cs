using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Common;
using Corvus.Domain.Enums;
using MediatR;

namespace Corvus.Application.Queries.Tenants;

public sealed record GetTenantsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Name = null,
    EntityStatusFilter Status = EntityStatusFilter.All) : IRequest<Result<PagedResult<TenantDto>>>;