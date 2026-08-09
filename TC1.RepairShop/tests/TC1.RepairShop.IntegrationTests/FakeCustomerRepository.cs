using System.Collections.Concurrent;
using TC1.RepairShop.Application.Registration;
using TC1.RepairShop.Domain.Common;
using TC1.RepairShop.Domain.Registration;

namespace TC1.RepairShop.IntegrationTests;

public class FakeCustomerRepository : ICustomerRepository
{
    public static readonly Customer SeedCustomer =
        Customer.Create("Jane Doe", "52998224725", "11999999999", "jane@example.com");

    private static readonly ConcurrentDictionary<Guid, Customer> Customers = new([
        new KeyValuePair<Guid, Customer>(SeedCustomer.Id, SeedCustomer),
    ]);

    public Task<Customer?> GetByNationalIdAsync(string nationalId)
    {
        var customer = Customers.Values.SingleOrDefault(c => c.NationalId == nationalId && c.Status != Status.Deleted);
        return Task.FromResult(customer);
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        Customers.TryGetValue(id, out var customer);
        return Task.FromResult(customer is not null && customer.Status != Status.Deleted ? customer : null);
    }

    public Task<IEnumerable<Customer>> GetAllAsync()
    {
        return Task.FromResult(Customers.Values.Where(c => c.Status != Status.Deleted));
    }

    public Task AddAsync(Customer customer)
    {
        Customers[customer.Id] = customer;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Customer customer)
    {
        Customers[customer.Id] = customer;
        return Task.CompletedTask;
    }
}
