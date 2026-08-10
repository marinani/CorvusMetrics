using Corvus.Domain.Entities;
using Corvus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Corvus.Infrastructure.Persistence.Repositories;

public sealed class MetricRepository : IMetricRepository
{
    private readonly CorvusDbContext _dbContext;

    public MetricRepository(CorvusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Metric?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Metrics
            .Include(metric => metric.Tags)
            .FirstOrDefaultAsync(metric => metric.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Metric>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Metrics
            .AsNoTracking()
            .Include(metric => metric.Tags)
            .OrderByDescending(metric => metric.RecordedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Metric entity, CancellationToken cancellationToken = default)
        => await _dbContext.Metrics.AddAsync(entity, cancellationToken);

    public void Update(Metric entity) => _dbContext.Metrics.Update(entity);

    public void Remove(Metric entity) => _dbContext.Metrics.Remove(entity);
}