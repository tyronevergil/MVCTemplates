using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class UserRole : EntityBase
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
