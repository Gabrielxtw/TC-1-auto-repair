using Dapper;
using TC1.RepairShop.Application.Auth;
using TC1.RepairShop.Domain.Auth;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public UserRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, Username, PasswordHash, Role
            FROM Users
            WHERE Username = @Username
            """;

        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }
}
