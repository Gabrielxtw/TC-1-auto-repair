using TC1.RepairShop.Domain.Entities.Services;

namespace TC1.RepairShop.Domain.Interfaces
{
    public interface IServiceRepository : IRepository<Service, Guid>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
