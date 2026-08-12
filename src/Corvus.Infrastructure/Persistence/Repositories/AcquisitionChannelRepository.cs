using Corvus.Domain.Common;
using Corvus.Domain.Entities;
using Corvus.Domain.Enums;
using Corvus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Corvus.Infrastructure.Persistence.Repositories;

public sealed class AcquisitionChannelRepository : RepositoryBase<AcquisitionChannel>, IAcquisitionChannelRepository
{
    public AcquisitionChannelRepository(CorvusDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => await DbContext.AcquisitionChannels
            .AnyAsync(channel => channel.Name == name, cancellationToken);

    public async Task<PagedResult<AcquisitionChannel>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? name,
        EntityStatusFilter status,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.AcquisitionChannels.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(channel => EF.Functions.ILike(channel.Name, $"%{name}%"));
        }

        query = ApplyStatusFilter(query, status);

        return await ApplyPaginationAsync(query, pageNumber, pageSize, cancellationToken);
    }

    private static IQueryable<AcquisitionChannel> ApplyStatusFilter(
        IQueryable<AcquisitionChannel> query,
        EntityStatusFilter status)
        => status switch
        {
            EntityStatusFilter.Active => query.Where(channel => channel.IsActive),
            EntityStatusFilter.Inactive => query.Where(channel => !channel.IsActive),
            _ => query
        };
}