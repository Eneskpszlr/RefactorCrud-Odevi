using CoreAPIForVB.Models1.Entities;
using Microsoft.EntityFrameworkCore;
using CoreAPIForVB.Extensions;
using CoreAPIForVB.Models1.Configurations;

namespace CoreAPIForVB.Models1.ContextClasses
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> opt) : base(opt) 
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.Seed(); //Extension metot ile kullanım
            //DataSeedStructure.AddCategories(modelBuilder); //normal static class ile kullanım
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
