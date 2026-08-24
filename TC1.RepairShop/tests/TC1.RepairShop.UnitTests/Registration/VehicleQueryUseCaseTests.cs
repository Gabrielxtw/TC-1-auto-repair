using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Vehicles.UseCases;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Registration;

public class VehicleQueryUseCaseTests
{
    private static Vehicle CreateVehicle(Guid customerId) =>
        Vehicle.Create(customerId, "ABC1234", "Toyota", "Corolla", 2022);

    [Fact]
    public async Task GetVehicleUseCase_ShouldReturnVehicle_WhenFound()
    {
        var vehicle = CreateVehicle(Guid.NewGuid());
        var repository = new Mock<IVehicleRepository>();
        repository.Setup(r => r.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);

        var useCase = new GetVehicleUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(vehicle.Id);

        result.Should().Be(vehicle);
    }

    [Fact]
    public async Task GetVehicleUseCase_ShouldReturnNull_WhenNotFound()
    {
        var repository = new Mock<IVehicleRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Vehicle?)null);

        var useCase = new GetVehicleUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task ListVehiclesUseCase_ShouldReturnAllVehicles_FromRepository()
    {
        var vehicle = CreateVehicle(Guid.NewGuid());
        var repository = new Mock<IVehicleRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { vehicle });

        var useCase = new ListVehiclesUseCase(repository.Object);
        var result = await useCase.ExecuteAsync();

        result.Should().ContainSingle().Which.Should().Be(vehicle);
    }

    [Fact]
    public async Task ListVehiclesByCustomerUseCase_ShouldReturnVehicles_ForGivenCustomer()
    {
        var customerId = Guid.NewGuid();
        var vehicle = CreateVehicle(customerId);
        var repository = new Mock<IVehicleRepository>();
        repository.Setup(r => r.GetByCustomerIdAsync(customerId)).ReturnsAsync(new[] { vehicle });

        var useCase = new ListVehiclesByCustomerUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(customerId);

        result.Should().ContainSingle().Which.Should().Be(vehicle);
    }
}
