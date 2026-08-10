using Corvus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corvus.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(user => user.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(user => user.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(user => user.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(user => user.IsActive)
            .IsRequired();

        builder.Ignore(user => user.DomainEvents);

        builder.HasIndex(user => user.Email)
            .IsUnique();
    }
}