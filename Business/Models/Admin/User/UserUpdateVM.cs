
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Admin.User
{
    public class UserUpdateVM
    {
        public UserUpdateVM()
        {
            RolesIds = new List<string>();
        }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }


        [Required(ErrorMessage = "  ad daxil edin ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "  soyad daxil edin ")]
        public string Surname { get; set; }

        public string? PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }


        [Compare(nameof(NewPassword), ErrorMessage = "şifrələr uyğunlaşmır")]
        [DataType(DataType.Password)]
        public string? NewConfirmPassword { get; set; }

        public List<SelectListItem>? Roles { get; set; }
        public List<string>? RolesIds { get; set; }
    }
}
