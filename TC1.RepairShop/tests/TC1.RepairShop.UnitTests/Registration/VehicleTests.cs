using System;
using Xunit;
using TC1.RepairShop.Domain.Entities.Registration;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.UnitTests.Registration;

public class VehicleTests
{
    [Fact]
    public void Create_ShouldInitializeVehicle()
    {
        var customerId = Guid.NewGuid();
        var vehicle = Vehicle.Create(customerId, "abc-1234", "Toyota", "Corolla", 2020);

        Assert.NotEqual(Guid.Empty, vehicle.Id);
        Assert.Equal(customerId, vehicle.CustomerId);
        Assert.Equal("ABC1234", vehicle.LicensePlate);
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
}
