
namespace Business.Models.Product
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string PhotoName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
