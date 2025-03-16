using System.Linq.Expressions;
using HardwareScrapper.Domain.Abstractions;

namespace HardwareScrapper.DataAccess.Abstractions
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T GetById(int id);
        T Add(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(int id);
        bool Exists(int id);
        int Count(Expression<Func<T, bool>> predicate = null);
        IEnumerable<T> GetPaged(int page, int pageSize, Expression<Func<T, bool>> predicate = null);
        IEnumerable<T> GetWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
