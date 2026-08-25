using FluentAssertions;
using Xunit;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Entities.Vehicles;

namespace TC1.RepairShop.UnitTests.Vehicles;

public class VehicleTests
{
    [Fact]
    public void Create_ShouldInitializeVehicle()
    {
        var customerId = Guid.NewGuid();
        var vehicle = Vehicle.Create(customerId, "abc-1234", "Toyota", "Corolla", 2020);

        Assert.NotEqual(Guid.Empty, vehicle.Id);
        Assert.Equal(customerId, vehicle.UserId);
        Assert.Equal("ABC1234", vehicle.LicensePlate.Value);
        Assert.Equal("Toyota", vehicle.Brand);
        Assert.Equal("Corolla", vehicle.Model);
        Assert.Equal(2020, vehicle.Year);
        Assert.Equal(Status.Active, vehicle.Status);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var vehicle = Vehicle.Create(Guid.NewGuid(), "ABC1234", "Ford", "Fiesta", 2018);

        vehicle.Delete();

        Assert.Equal(Status.Deleted, vehicle.Status);
    }

    [Fact]
    public void Create_ShouldThrow_WhenLicensePlateIsInvalid()
    {
        var act = () => Vehicle.Create(Guid.NewGuid(), "XYZ", "Ford", "Fiesta", 2018);

        act.Should().Throw<BusinessException>()
            .WithMessage("The license plate value must be a valid Brazilian license plate.");
    }
}
