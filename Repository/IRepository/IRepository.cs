using System.Linq.Expressions;
using WebApiDemo.Model;

namespace WebApiDemo.Repository.IRepository
{
    public interface IRepository<T> where T : class //genric class of type T istead of using villa
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeproperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeproperties = null);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task saveAsync();
    }
}
