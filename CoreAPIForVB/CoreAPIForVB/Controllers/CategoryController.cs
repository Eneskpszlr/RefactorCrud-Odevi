using CoreAPIForVB.Models1.Categories.RequestModels;
using CoreAPIForVB.Models1.Categories.ResponseModels;
using CoreAPIForVB.Models1.ContextClasses;
using CoreAPIForVB.Models1.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreAPIForVB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MyContext _db;

        public CategoryController(MyContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            List<CategoryResponseModel> categories = await _db.Categories.Select(x => new CategoryResponseModel
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
                Description = x.Description
            }).ToListAsync();

            return Ok(categories);
        }

        //todo: getById angular ödevi
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            CategoryResponseModel? category = await _db.Categories.Where(x => x.Id == id).Select(x => new CategoryResponseModel
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
                Description = x.Description
            }).FirstOrDefaultAsync();

            if (category == null)
                return Ok("Kategori bulunamadı");
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestModel model)
        {
            Category c = new()
            {
                CategoryName = model.CategoryName,
                Description = model.Description
            };
            await _db.Categories.AddAsync(c);
            await _db.SaveChangesAsync();
            return Ok("Kategori eklendi"); //Burada entity yerine bir mesaj döndürüyoruz. Angular'da bu bize özel bir header response'i
            // text olarak explicit şekilde almadığımız için Json serialize hatası verir onu gözlemleyiniz.


        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequestModel model)
        {
            Category? originalCategory = await _db.Categories.FindAsync(model.Id);
            originalCategory.CategoryName = model.CategoryName;
            originalCategory.Description = model.Description;
            await _db.SaveChangesAsync();

            return Ok($"{model.Id} id'li veriye yapılan güncelleme başarılı");
        }

        //Todo: ödev softdelete operasyonu

        //Todo : Cors

        //Eğer aşağıdaki sistemde FromBody prefix'i koymazsanız dış kaynaklar (Angular,React gibi yapılar) bu metottaki parametreye veri gönderemez
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory([FromBody]int id)
        {
            _db.Categories.Remove(await _db.Categories.FindAsync(id));
            await _db.SaveChangesAsync();
            return Ok($"{id} id'sine sahip veri silindi");
        }
    } //https://localhost:7292/api/Category
}
