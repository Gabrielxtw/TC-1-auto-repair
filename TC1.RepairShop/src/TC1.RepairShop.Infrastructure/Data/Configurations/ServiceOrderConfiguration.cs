using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Infrastructure.Data.Configurations;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> b)
    {
        b.ToTable("ServiceOrders");
        b.HasKey(o => o.Id);

        b.Property(o => o.OrderStatusValue)
            .HasConversion(
                v => v.Value,
                v => ServiceOrderStatus.FromValue(v)
            )
            .IsRequired();

        b.Property(o => o.OpenedAt).IsRequired();
        b.Property(o => o.CompletedAt);
        b.Property(o => o.QuoteId);

        // Indexes
        b.HasIndex(o => o.OpenedAt);

        b.HasOne<Quote>()
            .WithOne(q => q.ServiceOrder)
            .HasForeignKey<ServiceOrder>(o => o.QuoteId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasMany(s => s.Services)
            .WithMany(s => s.ServiceOrders)
            .UsingEntity<ServiceOrderService>();

        b.HasMany(s => s.Parts)
            .WithMany(s => s.ServiceOrders)
            .UsingEntity<ServiceOrderPart>();
    }
}
