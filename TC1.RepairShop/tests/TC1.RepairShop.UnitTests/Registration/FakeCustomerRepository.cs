using TC1.RepairShop.Application.Registration;
using TC1.RepairShop.Domain.Entities.Common;
using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.UnitTests.Registration;

public class FakeCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<Guid, Customer> _customers = [];

    public Task<Customer?> GetByNationalIdAsync(string nationalId)
    {
        var customer = _customers.Values.SingleOrDefault(c => c.NationalId == nationalId && c.Status != Status.Deleted);
        return Task.FromResult(customer);
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        _customers.TryGetValue(id, out var customer);
        return Task.FromResult(customer is not null && customer.Status != Status.Deleted ? customer : null);
    }

    public Task<IEnumerable<Customer>> GetAllAsync() =>
        Task.FromResult(_customers.Values.Where(c => c.Status != Status.Deleted));

    public Task AddAsync(Customer customer)
    {
        _customers[customer.Id] = customer;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Customer customer)
    {
        _customers[customer.Id] = customer;
        return Task.CompletedTask;
    }
}
