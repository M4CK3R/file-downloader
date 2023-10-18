using System.Linq.Expressions;

namespace Downloader.Data;

public interface IRepository<T> where T : Entity
{
    public Task<T?> GetByIdAsync(Guid id);
    public IQueryable<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate);
    public IQueryable<T> Get();
    public Task<T> AddAsync(T entity, bool noSave = false);
    public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, bool noSave = false);
    public Task<T> UpdateAsync(T entity, bool noSave = false);
    public Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, bool noSave = false);
    public Task<T> DeleteAsync(T entity, bool noSave = false);
    public Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<T> entities, bool noSave = false);
}