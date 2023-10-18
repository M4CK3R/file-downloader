using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Downloader.Data;

public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly DbSet<T> _dbSet;
    private readonly DbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public IQueryable<T> Get()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<T> AddAsync(T entity, bool noSave = false)
    {
        await _dbSet.AddAsync(entity);
        if (!noSave)
            await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, bool noSave = false)
    {
        await _dbSet.AddRangeAsync(entities);
        if (!noSave)
            await _context.SaveChangesAsync();
        return entities;
    }

    public async Task<T> UpdateAsync(T entity, bool noSave = false)
    {
        _context.Update(entity);
        if (!noSave)
            await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, bool noSave = false)
    {
        _context.UpdateRange(entities);
        if (!noSave)
            await _context.SaveChangesAsync();
        return entities;
    }

    public async Task<T> DeleteAsync(T entity, bool noSave = false)
    {
        _dbSet.Remove(entity);
        if (!noSave)
            await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<T> entities, bool noSave = false)
    {
        _dbSet.RemoveRange(entities);
        if (!noSave)
            await _context.SaveChangesAsync();
        return entities;
    }
}