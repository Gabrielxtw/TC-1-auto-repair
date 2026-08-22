using Microsoft.EntityFrameworkCore;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class QuoteRepository : GenericRepository<Quote>, IQuoteRepository
{
    public QuoteRepository(RepairShopDbContext context) : base(context)
    {
    }
}
