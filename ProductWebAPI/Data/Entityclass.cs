using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Model;

namespace ProductWebAPI.Data
{
    public class Entityclass:DbContext
    {
       public Entityclass(DbContextOptions<Entityclass>options):  base(options) { }
       public DbSet<Product> products { get; set; }
       public DbSet<Item> item { get; set; }
       public DbSet<Login> login { get; set; }
    }
}
