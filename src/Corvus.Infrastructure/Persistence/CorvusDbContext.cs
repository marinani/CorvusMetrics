using Corvus.Domain.Common;
using Corvus.Domain.Entities;
using Corvus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Corvus.Infrastructure.Persistence;

public sealed class CorvusDbContext : DbContext, IUnitOfWork
{
    public CorvusDbContext(DbContextOptions<CorvusDbContext> options)
        : base(options)
    {
    }

    public DbSet<Metric> Metrics => Set<Metric>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CorvusDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditTimestamps();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditTimestamps()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(entity => entity.CreatedAtUtc).CurrentValue = utcNow;
                    break;
                case EntityState.Modified:
                    entry.Property(entity => entity.UpdatedAtUtc).CurrentValue = utcNow;
                    break;
            }
        }
    }
}