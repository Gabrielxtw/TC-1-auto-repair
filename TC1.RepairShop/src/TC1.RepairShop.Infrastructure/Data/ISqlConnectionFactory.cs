using System.Data;

namespace TC1.RepairShop.Infrastructure.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
