using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.Tenants;

public sealed class GetActiveTenantsQueryHandler
    : IRequestHandler<GetActiveTenantsQuery, Result<IReadOnlyList<TenantDto>>>
{
    private readonly ITenantRepository _tenantRepository;

    public GetActiveTenantsQueryHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Result<IReadOnlyList<TenantDto>>> Handle(
        GetActiveTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var tenants = await _tenantRepository.GetActiveAsync(cancellationToken);

        var dtos = tenants
            .Select(tenant => new TenantDto(
                tenant.Id,
                tenant.Name,
                tenant.CNPJ,
                tenant.Email,
                tenant.IsActive))
            .ToList();

        return Result<IReadOnlyList<TenantDto>>.Success(dtos);
    }
}