
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Admin.User
{
    public class UserCreateVM
    {
        public UserCreateVM()
        {
            RolesIds = new List<string>();
        }
        [Required(ErrorMessage = " email adresi daxil edin ")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "  ad daxil edin ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "  soyad daxil edin ")]
        public string Surname { get; set; }

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = " şifrəni daxil edin")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = " eyniləşmə şifrəsini daxil edin")]
        [Compare(nameof(Password), ErrorMessage = "şifrələr uyğunlaşmır")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem>? Roles { get; set; }
        public List<string>? RolesIds { get; set; }
    }
}
