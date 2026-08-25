using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Infrastructure.Data;

namespace TC1.RepairShop.Infrastructure.Data.Repositories;

public class GenericRepository<T> : IRepository<T, Guid> where T : class
{
    protected readonly RepairShopDbContext _context;
    protected readonly DbSet<T> _set;

    public GenericRepository(RepairShopDbContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null) return;
        entity.GetType().GetProperty("Status")?.SetValue(entity, Enum.Parse(entity.GetType().GetProperty("Status")!.PropertyType, "Deleted"));
        await _context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _set.AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _set.FindAsync(id);
    }
    //public virtual async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
    //{
    //    IQueryable<T> query = _context.Set<T>();
    //    foreach (var include in includes)
    //    {
    //        query = query.Include(include);
    //    }
    //    return await query.FirstOrDefaultAsync(
    //        x => EF.Property<Guid>(x, "Id") == id);
    //}

    public virtual async Task UpdateAsync(T entity)
    {
        _set.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        var e = await GetByIdAsync(id);
        return e is not null;
    }

    public virtual async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
