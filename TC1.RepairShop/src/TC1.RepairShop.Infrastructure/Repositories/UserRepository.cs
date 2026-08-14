using Dapper;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Interfaces.Users;
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
            SELECT Id, Username, PasswordHash, Role, Status
            FROM Users
            WHERE Username = @Username AND Status != 'Deleted'
            """;

        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, Username, PasswordHash, Role, Status
            FROM Users
            WHERE Id = @Id AND Status != 'Deleted'
            """;

        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, Username, PasswordHash, Role, Status
            FROM Users
            WHERE Status != 'Deleted'
            """;

        return await connection.QueryAsync<User>(sql);
    }

    public async Task AddAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO Users (Id, Username, PasswordHash, Role, Status)
            VALUES (@Id, @Username, @PasswordHash, @Role, @Status)
            """;

        await connection.ExecuteAsync(sql, user);
    }

    public async Task UpdateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE Users
            SET Username = @Username, PasswordHash = @PasswordHash, Role = @Role, Status = @Status
            WHERE Id = @Id
            """;

        await connection.ExecuteAsync(sql, user);
    }
}
