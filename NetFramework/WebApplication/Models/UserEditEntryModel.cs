using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.Infrastructure;

namespace WebApplication.Models
{
    public class UserEditEntryModel
    {
        public virtual string Username { get; set; }

        [Required(ErrorMessage = "User Roles field is required.")]
        public virtual IEnumerable<string> UserRoles { get; set; }

        [RequiredIf("IsChangePassword", true, ErrorMessage = "Temporary Password field is required.")]
        public virtual string TempPassword { get; set; }

        [RequiredIf("IsChangePassword", true, ErrorMessage = "Confirm Password field is required.")]
        [Compare("TempPassword", ErrorMessage = "Temporary Password and Confirm Password are not equal.")]
        public virtual string ConfirmPassword { get; set; }

        public virtual bool IsChangePassword { get; set; }
    }
}
