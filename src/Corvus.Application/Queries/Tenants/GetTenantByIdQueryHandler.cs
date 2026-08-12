using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.Tenants;

public sealed class GetTenantByIdQueryHandler
    : IRequestHandler<GetTenantByIdQuery, Result<TenantDto>>
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantByIdQueryHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Result<TenantDto>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id, cancellationToken);

        if (tenant is null)
        {
            return Result<TenantDto>.Failure(
                new Error("Tenant.NotFound", $"Tenant with id '{request.Id}' was not found."));
        }

        var dto = new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.CNPJ,
            tenant.Email,
            tenant.IsActive);

        return Result<TenantDto>.Success(dto);
    }
}