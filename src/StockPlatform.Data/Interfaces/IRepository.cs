using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StockPlatform.Data.Interfaces
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);
        int Count();
        T GetOne(object id);
        T GetOne(Expression<Func<T, bool>> predicate);
        T GetOne(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Create(T entity);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        int Save();
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
