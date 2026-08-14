using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Interfaces.Parts;

namespace TC1.RepairShop.Infrastructure.Repositories
{
    public class PartRepository : IPartRepository
    {
        public Task AddAsync(Part part)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exist(string nome)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Part>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Part> GetByIdsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Part part)
        {
            throw new NotImplementedException();
        }
    }
}
