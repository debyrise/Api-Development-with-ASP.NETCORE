using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApiDemo.Data;
using WebApiDemo.Model;
using WebApiDemo.Repository.IRepository;

namespace WebApiDemo.Repository
{
    public class VillaRepository :Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _DbContent;

        public VillaRepository(ApplicationDbContext dbContent) : base (dbContent)
        {
            _DbContent = dbContent;
        }
        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _DbContent.Villas.Update(entity);
            await _DbContent.SaveChangesAsync();
            return entity;
        }

    }
}
