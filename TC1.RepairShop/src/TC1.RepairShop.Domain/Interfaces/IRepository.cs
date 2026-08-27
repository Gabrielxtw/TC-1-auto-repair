using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TC1.RepairShop.Domain.Interfaces
{
    public interface IRepository<T, TKey>
        where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(TKey id);
        //Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
        Task<bool> ExistsAsync(TKey id);
        Task SaveChangesAsync();
        Task Update(T entity);
        Task Add(T entity);
    }
}
