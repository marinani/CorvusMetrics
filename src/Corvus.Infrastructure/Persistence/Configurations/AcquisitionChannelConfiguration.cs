using Corvus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corvus.Infrastructure.Persistence.Configurations;

public sealed class AcquisitionChannelConfiguration : IEntityTypeConfiguration<AcquisitionChannel>
{
    public void Configure(EntityTypeBuilder<AcquisitionChannel> builder)
    {
        builder.ToTable("acquisition_channels");

        builder.HasKey(channel => channel.Id);

        builder.Property(channel => channel.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(channel => channel.Color)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(channel => channel.IsActive)
            .IsRequired();

        builder.Ignore(channel => channel.DomainEvents);

        builder.HasIndex(channel => channel.Name)
            .IsUnique();
    }
}