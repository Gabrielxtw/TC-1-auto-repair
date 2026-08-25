using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Vehicles.UseCases;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.Registration;

public class DeleteVehicleUseCaseTests
{
    private static Vehicle CreateVehicle() =>
        Vehicle.Create(Guid.NewGuid(), "ABC1234", "Toyota", "Corolla", 2022);

    [Fact]
    public async Task ExecuteAsync_ShouldSucceed_WhenVehicleExists()
    {
        var vehicle = CreateVehicle();
        var repository = new Mock<IVehicleRepository>();
        repository.Setup(r => r.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);

        var useCase = new DeleteVehicleUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(vehicle.Id);

        result.success.Should().BeTrue();
        vehicle.Status.Should().Be(Status.Deleted);
        repository.Verify(r => r.UpdateAsync(vehicle), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenVehicleDoesNotExist()
    {
        var repository = new Mock<IVehicleRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Vehicle?)null);

        var useCase = new DeleteVehicleUseCase(repository.Object);
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        result.success.Should().BeFalse();
        result.error.Should().Be("Vehicle not found.");
    }
}
