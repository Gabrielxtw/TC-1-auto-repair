using Dapper;
using TC1.RepairShop.Application.Registration;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public VehicleRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, CustomerId, LicensePlate, Brand, Model, Year, Status
            FROM Vehicles
            WHERE LicensePlate = @LicensePlate AND Status != 'Deleted'
            """;

        return await connection.QuerySingleOrDefaultAsync<Vehicle>(sql, new { LicensePlate = licensePlate });
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, CustomerId, LicensePlate, Brand, Model, Year, Status
            FROM Vehicles
            WHERE Id = @Id AND Status != 'Deleted'
            """;

        return await connection.QuerySingleOrDefaultAsync<Vehicle>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(Guid customerId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, CustomerId, LicensePlate, Brand, Model, Year, Status
            FROM Vehicles
            WHERE CustomerId = @CustomerId AND Status != 'Deleted'
            """;

        return await connection.QueryAsync<Vehicle>(sql, new { CustomerId = customerId });
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT Id, CustomerId, LicensePlate, Brand, Model, Year, Status
            FROM Vehicles
            WHERE Status != 'Deleted'
            """;

        return await connection.QueryAsync<Vehicle>(sql);
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO Vehicles (Id, CustomerId, LicensePlate, Brand, Model, Year, Status)
            VALUES (@Id, @CustomerId, @LicensePlate, @Brand, @Model, @Year, @Status)
            """;

        await connection.ExecuteAsync(sql, vehicle);
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE Vehicles
            SET CustomerId = @CustomerId, LicensePlate = @LicensePlate, Brand = @Brand,
                Model = @Model, Year = @Year, Status = @Status
            WHERE Id = @Id
            """;

        await connection.ExecuteAsync(sql, vehicle);
    }

    Task<Vehicle?> IVehicleRepository.GetByLicensePlateAsync(string licensePlate)
    {
        throw new NotImplementedException();
    }

    Task<Vehicle?> IVehicleRepository.GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Vehicle>> IVehicleRepository.GetByCustomerIdAsync(Guid customerId)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Vehicle>> IVehicleRepository.GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
