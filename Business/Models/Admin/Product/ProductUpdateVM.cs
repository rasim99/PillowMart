using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace Business.Models.Admin.Product
{
    public class ProductUpdateVM
    {
        [Required(ErrorMessage = "Ad daxil edin ")]
        [MinLength(3, ErrorMessage = "Minimum 3 xarakter olmalıdır")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Təsvir əlavə edinn")]
        [MinLength(8, ErrorMessage = "Minimum 8 xarakter olmalıdır")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Stok sayını daxil edin")]
        [Range(1, 1000, ErrorMessage = "Say  1 => 1000 aralığında olmalıdır ")]
        public int StockQuantity { get; set; }


        [Required(ErrorMessage = "Qiymət daxil edin")]
        [Range(10, 20000, ErrorMessage = "Qiymət  10 => 20000 aralığında olmalıdır")]
        public decimal Price { get; set; }

        public string? PhotoName { get; set; }

        public IFormFile? Photo { get; set; }

        [Required(ErrorMessage = "Kateqoriyanı seçin")]
        [Display(Name = "Category ")]
        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
