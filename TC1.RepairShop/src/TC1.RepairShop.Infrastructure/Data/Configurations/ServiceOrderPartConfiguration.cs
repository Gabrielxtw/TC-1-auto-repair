using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Entities.ServiceOrders;

namespace TC1.RepairShop.Infrastructure.Data.Configurations;

public class ServiceOrderPartConfiguration : IEntityTypeConfiguration<ServiceOrderPart>
{
    public void Configure(EntityTypeBuilder<ServiceOrderPart> b)
    {
        b.ToTable("ServiceOrderParts");
        b.HasKey(p => p.Id);

        b.Property(p => p.Quantity).IsRequired();
        b.Property(p => p.SuppliedByCustomer).IsRequired();

        b.HasOne(p => p.ServiceOrder)
            .WithMany(so => so.ServiceOrderParts)
            .HasForeignKey(p => p.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(p => p.Part)
            .WithMany(pa => pa.ServiceOrderParts)
            .HasForeignKey(p => p.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(p => p.ServiceOrderId);
        b.HasIndex(p => p.PartId);
    }
}
