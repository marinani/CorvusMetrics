using Corvus.Application.Common.Results;
using Corvus.Application.Common.Rules;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.Users;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return Result.Failure(
                new Error("User.NotFound", $"User with id '{request.Id}' was not found."));
        }

        var tenantIds = request.TenantIds ?? Array.Empty<Guid>();

        var ruleResult = UserTenantRules.Validate(request.Role, tenantIds);

        if (ruleResult.IsFailure)
        {
            return Result.Failure(ruleResult.Error!);
        }

        foreach (var tenantId in tenantIds)
        {
            if (!await _tenantRepository.ExistsByIdAsync(tenantId, cancellationToken))
            {
                return Result.Failure(
                    new Error("Tenant.NotFound", $"Tenant with id '{tenantId}' was not found."));
            }
        }

        user.Update(request.FirstName, request.LastName, request.Role);
        user.ClearTenants();

        foreach (var tenantId in tenantIds)
        {
            user.AddTenant(tenantId);
        }

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}