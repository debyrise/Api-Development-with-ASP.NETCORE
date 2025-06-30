using Microsoft.EntityFrameworkCore;
using WebApiDemo.Model;

namespace WebApiDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext > options) : base (options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
    }
}
