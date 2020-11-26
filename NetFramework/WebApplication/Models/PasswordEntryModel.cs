using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class PasswordEntryModel
    {
        [Required(ErrorMessage = "Current Password field is required.")]
        public virtual string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password field is required.")]
        public virtual string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password field is required.")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password are not equal.")]
        public virtual string ConfirmPassword { get; set; }
    }
}
