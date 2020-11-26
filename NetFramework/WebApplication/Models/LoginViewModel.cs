using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class LoginViewModel : LoginEntryModel
    {
        [Display(Name = "Username:", Prompt = "Username")]
        public override string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password:", Prompt = "Password")]
        public override string Password { get; set; }

        [Display(Name = "Remember me?")]
        public override bool RememberMe { get; set; }
    }
}
