

using System.ComponentModel.DataAnnotations;

namespace Business.Models.Admin.Account
{
    public class AccountLoginVM
    {
        [Required(ErrorMessage = " email adresi daxil edin ")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }


        [Required(ErrorMessage = " şifrəni daxil edin")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
