using TC1.RepairShop.Domain.Entities.Services;

namespace TC1.RepairShop.Domain.Entities.Services.Interfaces;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service> GetByIdsAsync(Guid id);
    Task AddAsync(Service part);
    Task UpdateAsync(Service part);
    Task<bool> Exist(string nome);
}
