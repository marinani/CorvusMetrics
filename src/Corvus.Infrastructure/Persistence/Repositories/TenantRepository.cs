using Corvus.Domain.Common;
using Corvus.Domain.Entities;
using Corvus.Domain.Enums;
using Corvus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Corvus.Infrastructure.Persistence.Repositories;

public sealed class TenantRepository : RepositoryBase<Tenant>, ITenantRepository
{
    public TenantRepository(CorvusDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<Tenant?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await DbContext.Tenants
            .FirstOrDefaultAsync(tenant => tenant.Email == email, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await DbContext.Tenants
            .AnyAsync(tenant => tenant.Email == email, cancellationToken);

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await DbContext.Tenants
            .AnyAsync(tenant => tenant.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Tenant>> GetActiveAsync(CancellationToken cancellationToken = default)
        => await DbContext.Tenants
            .AsNoTracking()
            .Where(tenant => tenant.IsActive)
            .OrderBy(tenant => tenant.Name)
            .ToListAsync(cancellationToken);

    public async Task<PagedResult<Tenant>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? name,
        EntityStatusFilter status,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Tenants.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(tenant => EF.Functions.ILike(tenant.Name, $"%{name}%"));
        }

        query = ApplyStatusFilter(query, status);

        return await ApplyPaginationAsync(query, pageNumber, pageSize, cancellationToken);
    }

    private static IQueryable<Tenant> ApplyStatusFilter(IQueryable<Tenant> query, EntityStatusFilter status)
        => status switch
        {
            EntityStatusFilter.Active => query.Where(tenant => tenant.IsActive),
            EntityStatusFilter.Inactive => query.Where(tenant => !tenant.IsActive),
            _ => query
        };
}