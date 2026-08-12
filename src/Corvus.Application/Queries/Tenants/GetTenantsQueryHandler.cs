using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Common;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.Tenants;

public sealed class GetTenantsQueryHandler
    : IRequestHandler<GetTenantsQuery, Result<PagedResult<TenantDto>>>
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantsQueryHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Result<PagedResult<TenantDto>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var paged = await _tenantRepository.GetPagedAsync(
            pageNumber,
            pageSize,
            request.Name,
            request.Status,
            cancellationToken);

        var dtos = paged.Items
            .Select(tenant => new TenantDto(
                tenant.Id,
                tenant.Name,
                tenant.CNPJ,
                tenant.Email,
                tenant.IsActive))
            .ToList();

        var result = new PagedResult<TenantDto>(dtos, pageNumber, pageSize, paged.TotalCount);

        return Result<PagedResult<TenantDto>>.Success(result);
    }
}