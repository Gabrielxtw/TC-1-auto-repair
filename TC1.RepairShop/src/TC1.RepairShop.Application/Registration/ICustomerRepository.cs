using TC1.RepairShop.Domain.Entities.Registration;

namespace TC1.RepairShop.Application.Registration;

public interface ICustomerRepository
{
    Task<Customer?> GetByNationalIdAsync(string nationalId);
    Task<Customer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
}
