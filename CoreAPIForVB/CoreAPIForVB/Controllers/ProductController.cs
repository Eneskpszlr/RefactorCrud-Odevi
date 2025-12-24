using CoreAPIForVB.Models1.ContextClasses;
using CoreAPIForVB.Models1.Entities;
using CoreAPIForVB.Models1.Products.RequestModels;
using CoreAPIForVB.Models1.Products.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreAPIForVB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MyContext _context;

        public ProductController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            List<ProductResponseModel> products = await _context.Products.Select(x => new ProductResponseModel
            {
                Id = x.Id,
                CategoryName = x.Category.CategoryName,
                ProductName = x.ProductName,
                CategoryId = x.CategoryId,
                UnitPrice = x.UnitPrice
            }).ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            ProductResponseModel? product = await _context.Products.Where(x => x.Id == id).Select(x => new ProductResponseModel
            {
                Id = x.Id,
                CategoryName = x.Category.CategoryName,
                ProductName = x.ProductName,
                CategoryId = x.CategoryId,
                UnitPrice = x.UnitPrice
            }).FirstOrDefaultAsync();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductRequestModel model)
        {
            Product p = new()
            {
                CategoryId = model.CategoryId,
                ProductName = model.ProductName,
                UnitPrice = model.UnitPrice
            };

            await _context.Products.AddAsync(p);
            await _context.SaveChangesAsync();

            return Ok("Ekleme başarılıdır.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequestModel model)
        {
            Product? guncellenecekVeri = await _context.Products.FindAsync(model.Id);

            guncellenecekVeri.ProductName = model.ProductName;
            guncellenecekVeri.UnitPrice = model.UnitPrice;
            guncellenecekVeri.CategoryId = model.CategoryId;
            guncellenecekVeri.UpdatedDate = DateTime.Now;
            guncellenecekVeri.Status = Models1.Enums.DataStatus.Updated;

            await _context.SaveChangesAsync();

            return Ok("Güncelleme başarılıdır.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromBody]int id)
        {
            Product? silinecek = await _context.Products.FindAsync(id);
            _context.Products.Remove(silinecek);
            await _context.SaveChangesAsync();
            return Ok("Veri silindi.");
        }
    }
}
