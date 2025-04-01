namespace Core.Entities
{
    public class BasketProduct:BaseEntity
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public bool HasChanged { get; set; }
    }
}
