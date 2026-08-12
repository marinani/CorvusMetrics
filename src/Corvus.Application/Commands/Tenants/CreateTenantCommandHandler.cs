using Corvus.Application.Common.Results;
using Corvus.Domain.Entities;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.Tenants;

public sealed class CreateTenantCommandHandler
    : IRequestHandler<CreateTenantCommand, Result<Guid>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        if (await _tenantRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return Result<Guid>.Failure(
                new Error("Tenant.EmailAlreadyExists", $"Email '{request.Email}' is already registered."));
        }

        var tenant = new Tenant(request.Name, request.CNPJ, request.Email);

        await _tenantRepository.AddAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(tenant.Id);
    }
}