using System.Linq.Expressions;
using HardwareScrapper.DataAccess.Contexts;
using HardwareScrapper.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HardwareScrapper.DataAccess.Abstractions
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly HardwareDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(HardwareDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.Where(e => e.IsActive).ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(e => e.IsActive).Where(predicate).ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            _context.SaveChanges();
            return entities;
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                // Soft delete
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        public bool Exists(int id)
        {
            return _dbSet.Any(e => e.Id == id && e.IsActive);
        }

        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            var query = _dbSet.Where(e => e.IsActive);
            if (predicate != null)
                query = query.Where(predicate);

            return query.Count();
        }

        public IEnumerable<T> GetPaged(int page, int pageSize, Expression<Func<T, bool>> predicate = null)
        {
            var query = _dbSet.Where(e => e.IsActive);

            if (predicate != null)
                query = query.Where(predicate);

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<T> GetWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.Where(e => e.IsActive).Where(predicate);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.ToList();
        }
    }
}
