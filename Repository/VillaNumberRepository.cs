using WebApiDemo.Data;
using WebApiDemo.Model;
using WebApiDemo.Repository.IRepository;

namespace WebApiDemo.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _DbContent;

        public VillaNumberRepository(ApplicationDbContext dbContent) : base(dbContent)
        {
            _DbContent = dbContent;
        }
        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _DbContent.VillaNumbers.Update(entity);
            await _DbContent.SaveChangesAsync();
            return entity;
        }
    }
}
