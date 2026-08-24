using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Application.Vehicles.UseCases;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.IntegrationTests;
using Xunit;

namespace TC1.RepairShop.UnitTests.Vehicles;

public class VehicleUseCaseTests
{
    private const string ValidNationalId = "52998224725";

    private static async Task<Guid> CreateUserAsync(FakeUserRepository userRepository)
    {
        var useCase = new CreateUserUseCase(userRepository);
        var result = await useCase.ExecuteAsync(
            new CreateUserRequest("Jane Doe", "Pass123", ValidNationalId, "jane@example.com", UserRole.Staff, "11999999999"));
        return result.data?.id ?? Guid.Empty;
    }

    [Fact]
    public async Task CreateVehicle_WithKnownUser_ShouldSucceed()
    {
        var userRepository = new FakeUserRepository();
        var vehicleRepository = new FakeVehicleRepository();
        var userId = await CreateUserAsync(userRepository);

        var useCase = new CreateVehicleUseCase(userRepository, vehicleRepository);
        var result = await useCase.ExecuteAsync(new CreateVehicleRequest(userId, "ABC1234", "Toyota", "Corolla", 2022));

        Assert.True(result.Success);
        Assert.NotNull(result.Vehicle);
    }

    [Fact]
    public async Task CreateVehicle_WithUnknownUser_ShouldFail()
    {
        var useCase = new CreateVehicleUseCase(new FakeUserRepository(), new FakeVehicleRepository());

        var result = await useCase.ExecuteAsync(new CreateVehicleRequest(Guid.NewGuid(), "ABC1234", "Toyota", "Corolla", 2022));

        Assert.False(result.Success);
        Assert.Equal("User not found.", result.Error);
    }

    [Fact]
    public async Task CreateVehicle_WithDuplicateLicensePlate_ShouldFail()
    {
        var userRepository = new FakeUserRepository();
        var vehicleRepository = new FakeVehicleRepository();
        var userId = await CreateUserAsync(userRepository);

        var useCase = new CreateVehicleUseCase(userRepository, vehicleRepository);
        await useCase.ExecuteAsync(new CreateVehicleRequest(userId, "ABC1234", "Toyota", "Corolla", 2022));

        var result = await useCase.ExecuteAsync(new CreateVehicleRequest(userId, "ABC1234", "Honda", "Civic", 2021));

        Assert.False(result.Success);
        Assert.Equal("License plate is already registered.", result.Error);
    }
}
