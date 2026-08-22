using TC1.RepairShop.Domain.Entities.Parts;

namespace TC1.RepairShop.Domain.Interfaces
{
    public interface IPartRepository: IRepository<Part, Guid>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
