using TC1.RepairShop.Application.Registration.UseCases;
using Xunit;

namespace TC1.RepairShop.UnitTests.Registration;

public class VehicleUseCaseTests
{
    private const string ValidNationalId = "52998224725";

    private static async Task<Guid> CreateCustomerAsync(FakeCustomerRepository customerRepository)
    {
        var useCase = new CreateCustomerUseCase(customerRepository);
        var result = await useCase.ExecuteAsync(
            new CreateCustomerRequest("Jane Doe", ValidNationalId, "11999999999", "jane@example.com"));
        return result.Customer!.Id;
    }

    [Fact]
    public async Task CreateVehicle_WithKnownCustomer_ShouldSucceed()
    {
        var customerRepository = new FakeCustomerRepository();
        var vehicleRepository = new FakeVehicleRepository();
        var customerId = await CreateCustomerAsync(customerRepository);

        var useCase = new CreateVehicleUseCase(customerRepository, vehicleRepository);
        var result = await useCase.ExecuteAsync(new CreateVehicleRequest(customerId, "ABC1234", "Toyota", "Corolla", 2022));

        Assert.True(result.Success);
        Assert.NotNull(result.Vehicle);
    }

    [Fact]
    public async Task CreateVehicle_WithUnknownCustomer_ShouldFail()
    {
        var useCase = new CreateVehicleUseCase(new FakeCustomerRepository(), new FakeVehicleRepository());

        var result = await useCase.ExecuteAsync(new CreateVehicleRequest(Guid.NewGuid(), "ABC1234", "Toyota", "Corolla", 2022));

        Assert.False(result.Success);
        Assert.Equal("Customer not found.", result.Error);
    }

    [Fact]
    public async Task CreateVehicle_WithDuplicateLicensePlate_ShouldFail()
    {
        var customerRepository = new FakeCustomerRepository();
        var vehicleRepository = new FakeVehicleRepository();
        var customerId = await CreateCustomerAsync(customerRepository);

        var useCase = new CreateVehicleUseCase(customerRepository, vehicleRepository);
        await useCase.ExecuteAsync(new CreateVehicleRequest(customerId, "ABC1234", "Toyota", "Corolla", 2022));

        var result = await useCase.ExecuteAsync(new CreateVehicleRequest(customerId, "ABC1234", "Honda", "Civic", 2021));

        Assert.False(result.Success);
        Assert.Equal("License plate is already registered.", result.Error);
    }
}
