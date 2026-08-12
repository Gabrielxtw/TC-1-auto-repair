using TC1.RepairShop.Domain.Services;

namespace TC1.RepairShop.Application.Services;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service> GetByIdsAsync(Guid id);
    Task AddAsync(Service part);
    Task UpdateAsync(Service part);
    Task<bool> Exist(string nome);
}
