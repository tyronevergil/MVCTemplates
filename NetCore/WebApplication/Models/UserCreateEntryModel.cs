using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class UserCreateEntryModel
    {
        [Required(ErrorMessage = "Username field is required.")]
        public virtual string Username { get; set; }

        [Required(ErrorMessage = "User Roles field is required.")]
        public virtual IEnumerable<string> UserRoles { get; set; }

        [Required(ErrorMessage = "Temporary Password field is required.")]
        public virtual string TempPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password field is required.")]
        [Compare("TempPassword", ErrorMessage = "Temporary Password and Confirm Password are not equal.")]
        public virtual string ConfirmPassword { get; set; }
    }
}
