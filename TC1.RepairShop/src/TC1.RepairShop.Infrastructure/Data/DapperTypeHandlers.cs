using Dapper;
using TC1.RepairShop.Domain.Entities.Clients;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Entities.ServiceOrders;

namespace TC1.RepairShop.Infrastructure.Data;

public static class DapperTypeHandlers
{
    public static void Register()
    {
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<Status>());
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<Role>());
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<QuoteStatus>());
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<ServiceOrderStatus>());
    }
}
