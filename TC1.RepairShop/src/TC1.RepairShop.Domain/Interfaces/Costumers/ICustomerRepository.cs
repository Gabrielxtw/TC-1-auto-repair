using TC1.RepairShop.Domain.Entities.Costumers;

namespace TC1.RepairShop.Application.Registration;

public interface ICostumerRepository
{
    Task<Costumer?> GetByNationalIdAsync(string nationalId);
    Task<Costumer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Costumer>> GetAllAsync();
    Task AddAsync(Costumer Costumer);
    Task UpdateAsync(Costumer Costumer);
}
