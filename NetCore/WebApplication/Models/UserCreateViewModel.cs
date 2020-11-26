using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication.Models
{
    public class UserCreateViewModel : UserCreateEntryModel
    {
        public UserCreateViewModel(IEnumerable<Persistence.Entities.Role> roles)
        {
            UserRoles = Enumerable.Empty<string>();
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

        public IEnumerable<string> Roles { get; private set; }
    }
}
