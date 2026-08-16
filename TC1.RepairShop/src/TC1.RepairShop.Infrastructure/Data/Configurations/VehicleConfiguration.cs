using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Registration;
using TC1.RepairShop.Domain.Entities.Users;

namespace TC1.RepairShop.Infrastructure.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> b)
    {
        b.ToTable("Vehicles");
        b.HasKey(v => v.Id);
        b.Property(v => v.Brand).HasMaxLength(100);
        b.Property(v => v.Model).HasMaxLength(100);
        b.Property(v => v.Year);

        b.Property(v => v.LicensePlate)
            .HasConversion(
                lp => lp.Value,
                v => LicensePlate.Create(v))
            .HasMaxLength(20)
            .IsRequired();

        b.HasIndex(v => v.LicensePlate).IsUnique();

        b.Property(v => v.UserId).IsRequired();
        b.HasOne<User>().WithMany().HasForeignKey(v => v.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
