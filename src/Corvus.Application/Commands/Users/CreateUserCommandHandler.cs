using Corvus.Application.Authentication.Abstractions;
using Corvus.Application.Common.Results;
using Corvus.Application.Common.Rules;
using Corvus.Domain.Entities;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.Users;

public sealed class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        ITenantRepository tenantRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return Result<Guid>.Failure(
                new Error("User.EmailAlreadyExists", $"Email '{request.Email}' is already registered."));
        }

        var tenantIds = request.TenantIds ?? Array.Empty<Guid>();

        var ruleResult = UserTenantRules.Validate(request.Role, tenantIds);

        if (ruleResult.IsFailure)
        {
            return Result<Guid>.Failure(ruleResult.Error!);
        }

        foreach (var tenantId in tenantIds)
        {
            if (!await _tenantRepository.ExistsByIdAsync(tenantId, cancellationToken))
            {
                return Result<Guid>.Failure(
                    new Error("Tenant.NotFound", $"Tenant with id '{tenantId}' was not found."));
            }
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            passwordHash,
            request.Role);

        foreach (var tenantId in tenantIds)
        {
            user.AddTenant(tenantId);
        }

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}