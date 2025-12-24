namespace CoreAPIForVB.Models1.Products.RequestModels
{
    public class UpdateProductRequestModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int? CategoryId { get; set; }
    }
}
