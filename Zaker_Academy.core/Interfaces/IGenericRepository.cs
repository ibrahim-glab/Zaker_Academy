using System.Linq.Expressions;

namespace Zaker_Academy.core.Interfaces
{
    public interface IGenericRepository<T> where T : class 
    {
        Task<IEnumerable<T>> GetAll(string[] relatedEntities = null);
        Task<IEnumerable<TType>> GetAll<TType>(Expression<Func<T, TType>> select , string[] relatedEntities = null) where TType : class;

        Task Add(T entity);

        Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> condition);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> condition, string[] relatedEntities = null);
        Task<ICollection<TType>> GetByCondition<TType>(Expression<Func<T, bool>> condition,Expression<Func<T, TType>> select , string[] relatedEntities = null) where TType : class;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        Task<ICollection<TType>> GetPagedAsync<TType>(
    Expression<Func<T, bool>> condition,
     Expression<Func<T, TType>> select,
    int pageNumber,
    int pageSize,
    Expression<Func<T, object>> orderBy,
    bool ascending = true) where TType : class;

        Task<bool> ExistsAsync(Expression<Func<T, bool>> condition);

        Task BulkInsert(IEnumerable<T> entities);

        Task BulkUpdate(IEnumerable<T> entities);

        Task BulkDelete(IEnumerable<T> entities);

        Task Update(T entity);

        Task Delete(T entity);

        Task<T?> GetByIdAsync(int id);
    }
}