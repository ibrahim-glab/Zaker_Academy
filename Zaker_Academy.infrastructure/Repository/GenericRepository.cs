using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Zaker_Academy.core.Interfaces;

namespace Zaker_Academy.infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)

        {
            _context = context;
        }

        public async Task BulkDelete(IEnumerable<T> entities)
        {
            try
            {
                _context.Set<T>().RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // log this exception in the future
            }
        }

        public async Task BulkInsert(IEnumerable<T> entities)
        {
            try
            {
                await _context.Set<T>().AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // log this exception in the future
            }
        }

        public async Task BulkUpdate(IEnumerable<T> entities)
        {
            try
            {
                _context.Set<T>().UpdateRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // log this exception in the future
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> condition)
        {
            var result = await _context.Set<T>().SingleOrDefaultAsync(condition);

            return result != null;
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            T? t = await _context.Set<T>().FindAsync(id);
            return t;
        }

        public async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> condition, int pageNumber, int pageSize, Expression<Func<T, object>> orderBy, bool ascending = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            query = ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);

            var resultList = await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> condition)
        {
            return await _context.Set<T>().Where(condition).ToListAsync();
        }

        async Task IGenericRepository<T>.Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        async Task IGenericRepository<T>.Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

      
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        async Task IGenericRepository<T>.Update(T entity)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _context.Set<T>().Update(entity);
        }

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> condition, string[] relatedEntities = null)
        {
            IQueryable<T> res = _context.Set<T>();

            if (relatedEntities != null)
            {
                foreach (var related in relatedEntities)
                {
                    res = res.Include(related);
                }
            }

            return await res.Where(condition).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<ICollection<TType>> GetByCondition<TType>(Expression<Func<T, bool>> condition, Expression<Func<T, TType>> select, string[] relatedEntities = null) where TType : class
        {
            IQueryable<T> res = _context.Set<T>();
            if (relatedEntities != null)
            {
                foreach (var related in relatedEntities)
                {
                    res = res.Include(related);
                }
            }
            return await res.Where(condition).Select(select).ToListAsync();
        }

      
        public async Task<IEnumerable<TType>> GetAll<TType>(Expression<Func<T, TType>> select, string[] relatedEntities = null) where TType :class
        {
            IQueryable<T> res = _context.Set<T>();
            if (relatedEntities != null)
            {
                foreach (var related in relatedEntities)
                {
                    res = res.Include(related);
                }
            }
            return await res.Select(select).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(string[] relatedEntities = null)
        {
            IQueryable<T> res = _context.Set<T>();
            if (relatedEntities != null)
            {
                foreach (var related in relatedEntities)
                {
                    res = res.Include(related);
                }
            }
            return await res.AsNoTracking().ToListAsync();
        }
    }
}