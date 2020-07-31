using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Model
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your email.")]
        [RegularExpression(@"^[\w+][\w\.\-]+@[\w\-]+(\.\w{2,4})+$|^\d{4}(\-)?\d{6}$|^91\-?\d{4}\-?\d{6}$", ErrorMessage = "Email invalid format.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        [UIHint("stringPassword")]
        [RegularExpression(@"[^<>]*", ErrorMessage = "The password format is incorrect.")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
