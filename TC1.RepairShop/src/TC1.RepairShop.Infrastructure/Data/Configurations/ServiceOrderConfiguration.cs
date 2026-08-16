using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Infrastructure.Data.Configurations;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> b)
    {
        b.ToTable("ServiceOrders");
        b.HasKey(o => o.Id);
        b.Property(o => o.UserId).IsRequired();
        b.Property(o => o.VehicleId).IsRequired();
        b.Property(o => o.OrderStatusValue)
            .HasConversion<string>()
            .IsRequired();
        b.Property(o => o.OpenedAt).IsRequired();
        b.Property(o => o.CompletedAt);
        b.Property(o => o.QuoteId);

        // Indexes
        b.HasIndex(o => o.OpenedAt);

        // Relationships
        b.HasOne<TC1.RepairShop.Domain.Entities.Users.User>().WithMany().HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<TC1.RepairShop.Domain.Entities.Vehicles.Vehicle>().WithMany().HasForeignKey(o => o.VehicleId).OnDelete(DeleteBehavior.Restrict);
    }
}
