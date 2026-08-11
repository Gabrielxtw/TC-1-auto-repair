using Dapper;
using TC1.RepairShop.Application.Registration;
using TC1.RepairShop.Domain.Registration;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CustomerRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Customer?> GetByNationalIdAsync(string nationalId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, Name, NationalId, Phone, Email, PasswordHash, RegisteredAt, Status
            FROM Customers
            WHERE NationalId = @NationalId AND Status != 'Deleted'
            """;

        return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { NationalId = nationalId });
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, Name, NationalId, Phone, Email, PasswordHash, RegisteredAt, Status
            FROM Customers
            WHERE Id = @Id AND Status != 'Deleted'
            """;

        return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, Name, NationalId, Phone, Email, PasswordHash, RegisteredAt, Status
            FROM Customers
            WHERE Status != 'Deleted'
            """;

        return await connection.QueryAsync<Customer>(sql);
    }

    public async Task AddAsync(Customer customer)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO Customers (Id, Name, NationalId, Phone, Email, PasswordHash, RegisteredAt, Status)
            VALUES (@Id, @Name, @NationalId, @Phone, @Email, @PasswordHash, @RegisteredAt, @Status)
            """;

        await connection.ExecuteAsync(sql, customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE Customers
            SET Name = @Name, NationalId = @NationalId, Phone = @Phone, Email = @Email,
                PasswordHash = @PasswordHash, Status = @Status
            WHERE Id = @Id
            """;

        await connection.ExecuteAsync(sql, customer);
    }
}
