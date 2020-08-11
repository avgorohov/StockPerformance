using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using StockPlatform.Data.Interfaces;
using StockPlatform.Data.Models;
using LinqKit;

namespace StockPlatform.Data.Repositories
{
    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        object IEntity.Id {
            get { return Id; }
            set { Id = (T)value; }
        }
    }

    public class EntityRepository<T> : IRepository<T>
            where T : class, IEntity, new()
    {

        private StockComparisonDbContext _context;

        public EntityRepository(StockComparisonDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryable(filter, orderBy, includeProperties, skip, take).AsEnumerable();
        }

        public virtual int Count()
        {
            return _context.Set<T>().Count();
        }

        public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsEnumerable();
        }

        public virtual T GetOne(object id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public virtual T GetOne(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual T GetOne(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefault();
        }

        //public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        //{
        //    return _context.Set<T>().Where(predicate);
        //}

        public virtual void Create(T entity)
        {
            //EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            //_context.Set<T>().Add(entity);
            _context.Set<T>().Add(entity);
        }

        public virtual void Update(T entity)
        {
            //EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            //dbEntityEntry.State = EntityState.Modified;

            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            //EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            //dbEntityEntry.State = EntityState.Deleted;

            var dbSet = _context.Set<T>();
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public virtual int Save()
        {
            return _context.SaveChanges();
        }  
        
        public virtual async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual IQueryable<T> GetQueryable<T>(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where T : class, IEntity
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public async virtual Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
    }
}
