using Corvus.Domain.Entities;

namespace Corvus.Domain.Interfaces;

public interface IMetricRepository : IRepository<Metric>
{
    Task<IReadOnlyList<Metric>> GetAllAsync(CancellationToken cancellationToken = default);
}