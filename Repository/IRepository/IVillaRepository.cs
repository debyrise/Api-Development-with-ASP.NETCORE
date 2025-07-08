using System.Linq.Expressions;
using WebApiDemo.Model;

namespace WebApiDemo.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);

    }
}
