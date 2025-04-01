using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Account;

public class ResetPasswordVM
{
    [Required(ErrorMessage = " yeni şifrəni daxil edin")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = " təsdiq şifrəsini daxil edin")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "şifrələr uyğunlaşmır")]
    public string ConfirmNewPassword { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }
}
