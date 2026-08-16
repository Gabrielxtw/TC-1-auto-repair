using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TC1.RepairShop.Domain.Entities.Services;

namespace TC1.RepairShop.Infrastructure.Data.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> b)
    {
        b.ToTable("Services");
        b.HasKey(s => s.Id);
        b.Property(s => s.Name).HasMaxLength(200).IsRequired();
        b.Property(s => s.Description).HasMaxLength(1000);
        b.Property(s => s.Price).HasColumnType("decimal(18,2)");
    }
}
