using Corvus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corvus.Infrastructure.Persistence.Configurations;

public sealed class UserTenantConfiguration : IEntityTypeConfiguration<UserTenant>
{
    public void Configure(EntityTypeBuilder<UserTenant> builder)
    {
        builder.ToTable("user_tenants");

        builder.HasKey(userTenant => userTenant.Id);

        builder.HasIndex(userTenant => new { userTenant.UserId, userTenant.TenantId })
            .IsUnique();

        builder.HasOne(userTenant => userTenant.User)
            .WithMany(user => user.UserTenants)
            .HasForeignKey(userTenant => userTenant.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(userTenant => userTenant.Tenant)
            .WithMany(tenant => tenant.UserTenants)
            .HasForeignKey(userTenant => userTenant.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(userTenant => userTenant.DomainEvents);
    }
}