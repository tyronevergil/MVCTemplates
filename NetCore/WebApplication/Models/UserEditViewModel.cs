using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication.Models
{
    public class UserEditViewModel : UserEditEntryModel
    {
        public UserEditViewModel(Persistence.Entities.User user, IEnumerable<Persistence.Entities.Role> roles)
        {
            Username = user.Username;
            UserRoles = user.Roles.Select(r => r.Role).ToList();
            Roles = roles.Select(r => r.Name).ToList();
        }

        [Display(Name = "Username:", Prompt = "Username")]
        public override string Username { get; set; }

        [Display(Name = "User Roles:")]
        public override IEnumerable<string> UserRoles { get; set; }

        [Display(Name = "Temporary Password:", Prompt = "Enter Password")]
        public override string TempPassword { get; set; }

        [Display(Name = "Confirm Password:", Prompt = "Confirm Password")]
        public override string ConfirmPassword { get; set; }

        [Display(Name = "Change Password")]
        public override bool IsChangePassword { get; set; }

        public IEnumerable<string> Roles { get; private set; }
    }
}
