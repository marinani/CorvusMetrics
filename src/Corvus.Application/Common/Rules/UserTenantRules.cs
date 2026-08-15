using Corvus.Application.Common.Results;
using Corvus.Domain.Enums;

namespace Corvus.Application.Common.Rules;

public static class UserTenantRules
{
    public static Result Validate(UserRole role, IReadOnlyCollection<Guid> tenantIds)
    {
        if (role == UserRole.Seller && tenantIds.Count != 1)
        {
            return Result.Failure(
                new Error("User.SellerRequiresOneTenant", "A seller must be linked to exactly one tenant."));
        }

        if (role == UserRole.Manager && tenantIds.Count < 1)
        {
            return Result.Failure(
                new Error("User.ManagerRequiresTenant", "A manager must be linked to at least one tenant."));
        }

        return Result.Success();
    }
}