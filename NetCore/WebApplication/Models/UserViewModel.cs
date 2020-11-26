using System;
using System.Linq;

namespace WebApplication.Models
{
    public class UserViewModel
    {
        public UserViewModel(Persistence.Entities.User user)
        {
            UserId = user.UserId;
            Username = user.Username;
            Roles = String.Join(", ", user.Roles.Select(r => r.Role).ToArray());
        }

        public int UserId { get; private set; }
        public string Username { get; private set; }
        public string Roles { get; private set; }
    }
}
