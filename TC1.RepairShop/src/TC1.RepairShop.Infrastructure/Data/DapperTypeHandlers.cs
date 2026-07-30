using Dapper;
using TC1.RepairShop.Domain.Common;
using TC1.RepairShop.Domain.Quotes;
using TC1.RepairShop.Domain.ServiceOrders;

namespace TC1.RepairShop.Infrastructure.Data;

public static class DapperTypeHandlers
{
    public static void Register()
    {
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<Status>());
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<QuoteStatus>());
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<ServiceOrderStatus>());
    }
}
