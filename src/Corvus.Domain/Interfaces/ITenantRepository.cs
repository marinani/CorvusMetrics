using Corvus.Domain.Common;
using Corvus.Domain.Entities;
using Corvus.Domain.Enums;

namespace Corvus.Domain.Interfaces;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<Tenant?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<PagedResult<Tenant>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? name,
        EntityStatusFilter status,
        CancellationToken cancellationToken = default);
}