using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using MediatR;

namespace Corvus.Application.Queries.Tenants;

public sealed record GetTenantByIdQuery(Guid Id) : IRequest<Result<TenantDto>>;