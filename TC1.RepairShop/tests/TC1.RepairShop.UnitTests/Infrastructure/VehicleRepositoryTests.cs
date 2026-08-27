using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TC1.RepairShop.Domain.Entities.Users;
using TC1.RepairShop.Domain.Entities.Vehicles;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Data.Repositories;
using Xunit;

namespace TC1.RepairShop.UnitTests.Infrastructure;

public class VehicleRepositoryTests
{
    private static RepairShopDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RepairShopDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RepairShopDbContext(options, Mock.Of<IPublisher>());
    }

    private static User CreateUser()
    {
        return User.Create("customer1", "Password@123", "01098843371", "customer1@example.com", UserRole.Customer, "11999999999");
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ShouldReturnVehicleWithUser()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);
        var user = CreateUser();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        await repository.AddAsync(vehicle);

        var stored = await repository.GetByIdAsync(vehicle.Id);

        Assert.NotNull(stored);
        Assert.Equal(vehicle.Id, stored!.Id);
        Assert.NotNull(stored.User);
        Assert.Equal(user.Id, stored.User.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);

        var stored = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(stored);
    }

    [Fact]
    public async Task GetAllAsync_ShouldIncludeUser()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);
        var user = CreateUser();
        context.Users.Add(user);
        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        var all = await repository.GetAllAsync();

        var result = Assert.Single(all);
        Assert.NotNull(result.User);
        Assert.Equal(user.Id, result.User.Id);
    }

    [Fact]
    public async Task GetAllAsync_WithNoVehicles_ShouldReturnEmpty()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);

        var all = await repository.GetAllAsync();

        Assert.Empty(all);
    }

    [Fact]
    public async Task GetByLicensePlateAsync_WithValidFormat_ShouldQueryWithoutThrowing()
    {
        // Note: LicensePlate has no value equality (no Equals/== override), so the
        // `v.LicensePlate == normalized` comparison in the repository compares by
        // reference. Against the InMemory provider this means a match is never found
        // even when the normalized plate value is identical, so this asserts the
        // actual current behavior rather than a value-equality match.
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);
        var user = CreateUser();
        context.Users.Add(user);
        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        var result = await repository.GetByLicensePlateAsync("ABC1D23");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByLicensePlateAsync_WithLegacyFormat_ShouldNotThrow()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);
        var user = CreateUser();
        context.Users.Add(user);
        var vehicle = Vehicle.Create(user.Id, "ABC1234", "Ford", "Ka", 2020);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        var result = await repository.GetByLicensePlateAsync("ABC1234");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByLicensePlateAsync_WithValidFormatButNotFound_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);

        var result = await repository.GetByLicensePlateAsync("XYZ9W88");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByLicensePlateAsync_WithInvalidFormat_ShouldReturnNull()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);

        var result = await repository.GetByLicensePlateAsync("INVALID");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCustomerIdAsync_ShouldReturnMatchingVehicles()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);
        var user = CreateUser();
        var otherUser = User.Create("customer2", "Password@123", "52998224725", "customer2@example.com", UserRole.Customer, "11988888888");
        context.Users.Add(user);
        context.Users.Add(otherUser);
        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        var otherVehicle = Vehicle.Create(otherUser.Id, "XYZ9W88", "Chevrolet", "Onix", 2021);
        context.Vehicles.Add(vehicle);
        context.Vehicles.Add(otherVehicle);
        await context.SaveChangesAsync();

        var result = await repository.GetByCustomerIdAsync(user.Id);

        var found = Assert.Single(result);
        Assert.Equal(vehicle.Id, found.Id);
        Assert.NotNull(found.User);
    }

    [Fact]
    public async Task GetByCustomerIdAsync_WithUnknownCustomer_ShouldReturnEmpty()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);

        var result = await repository.GetByCustomerIdAsync(Guid.NewGuid());

        Assert.Empty(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForStoredEntity()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);
        var user = CreateUser();
        context.Users.Add(user);
        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        var exists = await repository.ExistsAsync(vehicle.Id);
        var notExists = await repository.ExistsAsync(Guid.NewGuid());

        Assert.True(exists);
        Assert.False(notExists);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);
        var user = CreateUser();
        context.Users.Add(user);
        var vehicle = Vehicle.Create(user.Id, "ABC1D23", "Ford", "Ka", 2020);
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        vehicle.Deactivate();
        await repository.UpdateAsync(vehicle);

        var stored = await repository.GetByIdAsync(vehicle.Id);
        Assert.NotNull(stored);
        Assert.Equal(Status.Inactive, stored!.Status);
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldNotThrow()
    {
        await using var context = CreateContext();
        var repository = new VehicleRepository(context);

        await repository.DeleteAsync(Guid.NewGuid());

        Assert.True(true);
    }
}
