using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication.Models
{
    public class UserDeleteViewModel
    {
        public UserDeleteViewModel(Persistence.Entities.User user, IEnumerable<Persistence.Entities.Role> roles)
        {
            Username = user.Username;
            UserRoles = user.Roles.Select(r => r.Role).ToList();
            Roles = roles.Select(r => r.Name).ToList();
        }

        [Display(Name = "Username:", Prompt = "Username")]
        public string Username { get; set; }

        [Display(Name = "User Roles:")]
        public IEnumerable<string> UserRoles { get; set; }

        public IEnumerable<string> Roles { get; private set; }
    }
}
