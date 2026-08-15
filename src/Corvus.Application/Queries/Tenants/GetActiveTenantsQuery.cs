using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using MediatR;

namespace Corvus.Application.Queries.Tenants;

public sealed record GetActiveTenantsQuery : IRequest<Result<IReadOnlyList<TenantDto>>>;