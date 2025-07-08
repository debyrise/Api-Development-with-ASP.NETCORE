using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApiDemo.Data;
using WebApiDemo.Model;
using WebApiDemo.Repository.IRepository;


namespace WebApiDemo.Repository
{
    //genric repo
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _DbContent;
        internal DbSet<T> Dbset;

        public Repository(ApplicationDbContext dbContent)
        {
            _DbContent = dbContent;
            this.Dbset = _DbContent.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await Dbset.AddAsync(entity);
            await saveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = Dbset;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = Dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            Dbset.Remove(entity);
            await saveAsync();
        }

        public async Task saveAsync()
        {
            await _DbContent.SaveChangesAsync();
        }

        //public async Task UpdateAsync(T entity)
        //{
        //    Dbset.Update(entity);
        //    await saveAsync();
        //}
    }
}
