using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.ServiceOrders;

namespace TC1.RepairShop.Infrastructure.Data.Configurations;

public class ServiceOrderServiceConfiguration : IEntityTypeConfiguration<ServiceOrderService>
{
    public void Configure(EntityTypeBuilder<ServiceOrderService> b)
    {
        b.ToTable("ServiceOrderServices");
        b.HasKey(s => s.Id);

        b.Property(s => s.Price).IsRequired();

        b.HasOne(s => s.ServiceOrder)
            .WithMany(so => so.ServiceOrderServices)
            .HasForeignKey(s => s.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(s => s.Service)
            .WithMany(pa => pa.ServiceOrderServices)
            .HasForeignKey(s => s.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(s => s.ServiceOrderId);
        b.HasIndex(s => s.ServiceId);
    }
}
