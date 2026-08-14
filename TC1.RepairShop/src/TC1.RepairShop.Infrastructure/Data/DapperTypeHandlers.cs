using Dapper;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Infrastructure.Data;

public static class DapperTypeHandlers
{
    public static void Register()
    {
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<Status>());
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<UserRole>());
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<QuoteStatus>());
        SqlMapper.AddTypeHandler(new EnumStringTypeHandler<ServiceOrderStatus>());
    }
}
