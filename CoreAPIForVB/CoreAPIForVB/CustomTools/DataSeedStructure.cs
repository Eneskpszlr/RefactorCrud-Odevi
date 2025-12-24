using CoreAPIForVB.Models1.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreAPIForVB.CustomTools
{
    public static class DataSeedStructure
    {
        public static void AddCategories(ModelBuilder modelBuilder)
        {
            Category c = new()
            {
                Id = 1,
                CategoryName = "Tatlılar",
                Description = "Test verisidir."
            };

            modelBuilder.Entity<Category>().HasData(c);
        }
    }
}
