using System.Linq.Expressions;
using Corvus.Domain.Common;
using Corvus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Corvus.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly CorvusDbContext DbContext;

    protected RepositoryBase(CorvusDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await DbContext.Set<TEntity>()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = DbContext.Set<TEntity>().AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(entity => entity.CreatedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(items, pageNumber, pageSize, totalCount);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

    protected async Task<PagedResult<TEntity>> ApplyPaginationAsync(
        IQueryable<TEntity> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var count = await query.CountAsync(cancellationToken);

        query = query
            .OrderByDescending(entity => entity.CreatedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var items = await query.ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(items, pageNumber, pageSize, count);
    }

    public virtual void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

    public virtual void Remove(TEntity entity) => DbContext.Set<TEntity>().Remove(entity);
}