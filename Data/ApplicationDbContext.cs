using Microsoft.EntityFrameworkCore;
using WebApiDemo.Model;

namespace WebApiDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext > options) : base (options)
        {
            
        }
        public DbSet<LocalUser> localUsers { get; set; }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Villa>().HasData(
        //        new Villa
        //        {
        //            Id = 6,
        //            Name = "Diamond Villa",
        //            Details = "Reference site about Lorem Ipsum, giving information on its origins, as well as a random Lipsum generator.\r\n",
        //            ImageUrl = "https://images.unsplash.com/photo-1603477849227-705c424d1d80?fm=jpg&q=60&w=3000&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8dHJvcGljYWwlMjBiZWFjaHxlbnwwfHwwfHx8MA%3D%3D",
        //            Occupancy = 4,
        //            Rate = 600,
        //            Sqft = 1100,
        //            Amenity = ""
        //        }
        //        );
        //}
    }
}
