using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class Role : EntityBase
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
    }
}
