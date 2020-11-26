using System;
using System.Collections.Generic;
using CrudDatastore;

namespace Persistence.Entities
{
    public class User : EntityBase
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
    }
}
