using TC1.RepairShop.Application.Registration.UseCases;
using Xunit;

namespace TC1.RepairShop.UnitTests.Registration;

public class CustomerUseCaseTests
{
    private const string ValidNationalId = "52998224725";

    [Fact]
    public async Task CreateCustomer_WithNewNationalId_ShouldSucceed()
    {
        var useCase = new CreateCustomerUseCase(new FakeCustomerRepository());

        var result = await useCase.ExecuteAsync(
            new CreateCustomerRequest("Jane Doe", ValidNationalId, "11999999999", "jane@example.com"));

        Assert.True(result.Success);
        Assert.NotNull(result.Customer);
        Assert.True(result.Customer!.VerifyPassword("529Jane"));
    }

    [Fact]
    public async Task CreateCustomer_WithDuplicateNationalId_ShouldFail()
    {
        var repository = new FakeCustomerRepository();
        var useCase = new CreateCustomerUseCase(repository);
        await useCase.ExecuteAsync(new CreateCustomerRequest("Jane Doe", ValidNationalId, "11999999999", "jane@example.com"));

        var result = await useCase.ExecuteAsync(
            new CreateCustomerRequest("Other Name", ValidNationalId, "11988888888", "other@example.com"));

        Assert.False(result.Success);
        Assert.Equal("National ID is already registered.", result.Error);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldSoftDeleteAndHideFromGetById()
    {
        var repository = new FakeCustomerRepository();
        var createUseCase = new CreateCustomerUseCase(repository);
        var created = await createUseCase.ExecuteAsync(
            new CreateCustomerRequest("Jane Doe", ValidNationalId, "11999999999", "jane@example.com"));

        var deleteUseCase = new DeleteCustomerUseCase(repository);
        var result = await deleteUseCase.ExecuteAsync(created.Customer!.Id);

        Assert.True(result.Success);

        var getUseCase = new GetCustomerUseCase(repository);
        var fetched = await getUseCase.ExecuteAsync(created.Customer!.Id);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task ChangeCustomerPassword_WithCorrectCurrentPassword_ShouldSucceed()
    {
        var repository = new FakeCustomerRepository();
        var createUseCase = new CreateCustomerUseCase(repository);
        var created = await createUseCase.ExecuteAsync(
            new CreateCustomerRequest("Jane Doe", ValidNationalId, "11999999999", "jane@example.com"));

        var changeUseCase = new ChangeCustomerPasswordUseCase(repository);
        var result = await changeUseCase.ExecuteAsync(
            new ChangeCustomerPasswordRequest(created.Customer!.Id, "529Jane", "NewPassw0rd!"));

        Assert.True(result.Success);

        var getUseCase = new GetCustomerUseCase(repository);
        var updated = await getUseCase.ExecuteAsync(created.Customer.Id);
        Assert.True(updated!.VerifyPassword("NewPassw0rd!"));
    }

    [Fact]
    public async Task ChangeCustomerPassword_WithWrongCurrentPassword_ShouldFail()
    {
        var repository = new FakeCustomerRepository();
        var createUseCase = new CreateCustomerUseCase(repository);
        var created = await createUseCase.ExecuteAsync(
            new CreateCustomerRequest("Jane Doe", ValidNationalId, "11999999999", "jane@example.com"));

        var changeUseCase = new ChangeCustomerPasswordUseCase(repository);
        var result = await changeUseCase.ExecuteAsync(
            new ChangeCustomerPasswordRequest(created.Customer!.Id, "wrong-password", "NewPassw0rd!"));

        Assert.False(result.Success);
        Assert.Equal("Current password is incorrect.", result.Error);
    }
}
