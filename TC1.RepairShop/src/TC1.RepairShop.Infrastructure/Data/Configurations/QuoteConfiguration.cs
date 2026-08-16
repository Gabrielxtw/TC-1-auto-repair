using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Infrastructure.Data.Configurations;

public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> b)
    {
        b.ToTable("Quotes");
        b.HasKey(q => q.Id);
        b.Property(q => q.ServiceOrderId).IsRequired();
        b.Property(q => q.Price).HasColumnType("decimal(18,2)");
        b.Property(q => q.QuoteStatusValue)
            .HasConversion<string>()
            .IsRequired();
        b.Property(q => q.RejectionCount);

        // Relationship: One Quote -> One ServiceOrder
        b.HasOne(q => q.ServiceOrder).WithOne(o => o.Quote).HasForeignKey<Quote>(q => q.ServiceOrderId).OnDelete(DeleteBehavior.Cascade);
    }
}
