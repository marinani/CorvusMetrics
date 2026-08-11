using Corvus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corvus.Infrastructure.Persistence.Configurations
{
    public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");

            builder.HasKey(tenant => tenant.Id);

            builder.Property(tenant => tenant.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(tenant => tenant.CNPJ)
                .IsRequired()
                .HasMaxLength(14);

            builder.Property(tenant => tenant.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
