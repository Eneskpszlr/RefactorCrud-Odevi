using CoreAPIForVB.Models1.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreAPIForVB.Extensions
{
    public static class DataSeedExtension
    {
        //Primary key identity'si burada tetiklenmez. Dolayısıyla id'yi elle vermeliyiz
        public static void Seed(this ModelBuilder modelBuilder)
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
