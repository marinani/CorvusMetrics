using Corvus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corvus.Infrastructure.Persistence.Configurations;

public sealed class MetricConfiguration : IEntityTypeConfiguration<Metric>
{
    public void Configure(EntityTypeBuilder<Metric> builder)
    {
        builder.ToTable("metrics");

        builder.HasKey(metric => metric.Id);

        builder.Property(metric => metric.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(metric => metric.Value)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(metric => metric.Unit)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(metric => metric.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(metric => metric.Description)
            .HasMaxLength(500);

        builder.Property(metric => metric.RecordedAtUtc)
            .IsRequired();

        builder.Property(metric => metric.CreatedAtUtc)
            .IsRequired();

        builder.Property(metric => metric.UpdatedAtUtc);

        builder.OwnsMany(
            metric => metric.Tags,
            tag =>
            {
                tag.ToTable("metric_tags");
                tag.WithOwner().HasForeignKey("MetricId");
                tag.Property(t => t.Key).HasColumnName("tag_key").IsRequired().HasMaxLength(50);
                tag.Property(t => t.Value).HasColumnName("tag_value").IsRequired().HasMaxLength(100);
            });

        builder.Ignore(metric => metric.DomainEvents);

        builder.HasIndex(metric => metric.Name);
        builder.HasIndex(metric => metric.RecordedAtUtc);
    }
}