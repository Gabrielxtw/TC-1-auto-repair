using TC1.RepairShop.Domain.Entities.Parts;

namespace TC1.RepairShop.Domain.Interfaces.Parts
{
    public interface IPartRepository
    {
        Task<IEnumerable<Part>> GetAllAsync();
        Task<Part> GetByIdsAsync(Guid id);
        Task AddAsync(Part part);
        Task UpdateAsync(Part part);
        Task<bool> Exist(string nome);
    }
}
