
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Admin.Category
{
    public class CategoryUpdateVM
    {
        [Required(ErrorMessage ="Ad daxil edin")]
        public string Name { get; set; }
    }
}
