
using Core.Entities;

namespace Business.Models.Basket
{
    public class BasketIndexVM
    {
        public BasketIndexVM()
        {
            BasketProducts = new List<BasketProduct>();
        }
        public List<BasketProduct> BasketProducts { get; set; }
    }
}
