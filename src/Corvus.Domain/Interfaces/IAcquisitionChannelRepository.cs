using Corvus.Domain.Common;
using Corvus.Domain.Entities;
using Corvus.Domain.Enums;

namespace Corvus.Domain.Interfaces;

public interface IAcquisitionChannelRepository : IRepository<AcquisitionChannel>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<PagedResult<AcquisitionChannel>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? name,
        EntityStatusFilter status,
        CancellationToken cancellationToken = default);
}