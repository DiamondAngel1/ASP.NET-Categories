using Microsoft.EntityFrameworkCore;
using WorkingMVC.Data.Entitys;

namespace WorkingMVC.Data
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
            : base(options)
        {

        }
        public DbSet<CategoryEntity> Categories { get; set; }
    }
}