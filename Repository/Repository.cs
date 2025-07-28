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
            //_DbContent.VillaNumbers.Include(x => x.Villa).ToList();
            this.Dbset = _DbContent.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await Dbset.AddAsync(entity);
            await saveAsync();
        }
        //"villa,villaSpecial"

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeproperties = null)
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
           
            if (includeproperties != null)
            {
                foreach(var includeProp in includeproperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeproperties = null, int PageSize = 0, int PageNumber = 1)
        {
            IQueryable<T> query = Dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (PageSize > 0)
            {
                if (PageSize > 100)
                {
                    PageSize = 100;
                }
                //skip0.take(5)
                //page number-2 page size -5
                //skip(5*(1)) take(5)
                query = query.Skip(PageSize * (PageNumber - 1)).Take(PageSize);
            }

            if (includeproperties != null)
            {
                foreach (var includeProp in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
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
