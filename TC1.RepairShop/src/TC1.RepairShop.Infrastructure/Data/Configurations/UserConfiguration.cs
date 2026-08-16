using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TC1.RepairShop.Domain.Entities.Users;

namespace TC1.RepairShop.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(u => u.Id);
        b.Property(u => u.Username).HasMaxLength(100).IsRequired();
        b.HasIndex(u => u.Username).IsUnique();

        b.Property(u => u.Email)
            .HasConversion(
                v => v.Value,
                v => TC1.RepairShop.Domain.ValueObjects.Email.Create(v))
            .HasMaxLength(200)
            .IsRequired();

        b.Property(u => u.Document)
            .HasConversion(
                v => v.Value,
                v => TC1.RepairShop.Domain.ValueObjects.Document.Create(v))
            .HasMaxLength(20)
            .IsRequired();

        b.Property(u => u.Phone).HasMaxLength(20);
        b.HasIndex(u => u.Email).IsUnique();
        b.Property(u => u.PasswordHash).HasMaxLength(200).IsRequired();
        b.Property(u => u.Status).IsRequired();
    }
}
