using Corvus.Domain.Common;

namespace Corvus.Domain.Entities;

public sealed class UserTenant : BaseEntity
{
    private UserTenant()
    {
    }

    public UserTenant(Guid userId, Guid tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }

    public Guid UserId { get; private set; }

    public Guid TenantId { get; private set; }

    public User User { get; private set; } = null!;

    public Tenant Tenant { get; private set; } = null!;
}