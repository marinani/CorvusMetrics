using Corvus.Application.Common.Results;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.Tenants;

public sealed class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, Result>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTenantCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id, cancellationToken);

        if (tenant is null)
        {
            return Result.Failure(
                new Error("Tenant.NotFound", $"Tenant with id '{request.Id}' was not found."));
        }

        _tenantRepository.Remove(tenant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}