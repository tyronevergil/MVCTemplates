using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Models
{
    public class UsersViewModel
    {
        public UsersViewModel(IEnumerable<Persistence.Entities.User> users)
        {
            Users = users.Select(u => new UserViewModel(u)).ToList();
        }

        public IEnumerable<UserViewModel> Users { get; private set; }
    }
}
