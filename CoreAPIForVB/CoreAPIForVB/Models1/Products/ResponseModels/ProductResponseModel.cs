namespace CoreAPIForVB.Models1.Products.ResponseModels
{
    public class ProductResponseModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
