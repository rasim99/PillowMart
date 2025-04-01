using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Account;

public class ForgetPasswordVM
{
    [Required(ErrorMessage = " email adresi daxil edin ")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
