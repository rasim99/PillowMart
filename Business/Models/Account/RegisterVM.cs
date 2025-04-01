
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Account
{
    public class RegisterVM
    {
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


        [Required(ErrorMessage = " təsdiq şifrəsini daxil edin")]
        [Compare(nameof(Password),ErrorMessage = "şifrələr uyğunlaşmır")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
