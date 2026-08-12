using Corvus.Application.Common.Results;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.Tenants;

public sealed class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, Result>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTenantCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id, cancellationToken);

        if (tenant is null)
        {
            return Result.Failure(
                new Error("Tenant.NotFound", $"Tenant with id '{request.Id}' was not found."));
        }

        tenant.Update(request.Name, request.CNPJ, request.Email);

        _tenantRepository.Update(tenant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}