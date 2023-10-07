using System.Linq.Expressions;

namespace Zaker_Academy.core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task Add(T entity);

        Task<IEnumerable<T>> getByCondition(Expression<Func<T, bool>> condition);

        Task<IEnumerable<T>> GetPagedAsync(
    Expression<Func<T, bool>> condition,
    int pageNumber,
    int pageSize,
    Expression<Func<T, object>> orderBy,
    bool ascending = true);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> condition);

        Task BulkInsert(IEnumerable<T> entities);

        Task BulkUpdate(IEnumerable<T> entities);

        Task BulkDelete(IEnumerable<T> entities);

        Task Update(T entity);

        Task Delete(T entity);

        Task<T> GetByIdAsync(object id);
    }
}