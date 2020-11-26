using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class LoginEntryModel
    {
        [Required(ErrorMessage = "Username field is required.")]
        public virtual string Username { get; set; }

        [Required(ErrorMessage = "Password field is required.")]
        public virtual string Password { get; set; }

        public virtual bool RememberMe { get; set; }
    }
}
