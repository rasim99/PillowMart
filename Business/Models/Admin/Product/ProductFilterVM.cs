
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Admin.Product
{
    public class ProductFilterVM
    {
        public string? Name { get; set; }

        [Display(Name = "Minimum say")]
        public int? MinQuantity { get; set; }

        [Display(Name = "Maksimum say")]
        public int? MaxQuantity { get; set; }


        [Display(Name = "Minimum qiymət")]
        public decimal? MinPrice { get; set; }

        [Display(Name = "Maksimum qiymət")]
        public decimal? MaxPrice { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public List<int> CategoriesIds { get; set; }


        [Display(Name = "Başlanğıc  tarix")]
        public DateTime? CreatedStartDate { get; set; }

        [Display(Name = "Son tarix")]
        public DateTime? CreatedEndDate { get; set; }

        public List<Core.Entities.Product> Products { get; set; }
    }
}
