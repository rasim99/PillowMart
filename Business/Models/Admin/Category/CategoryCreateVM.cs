
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Admin.Category
{
    public class CategoryCreateVM
    {

        [Required(ErrorMessage ="Kateqoriya adını daxil edin")]
        public string Name { get; set; }
    }
}
