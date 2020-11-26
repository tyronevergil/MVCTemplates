using System;
using System.Linq.Expressions;
using CrudDatastore;

namespace Persistence.Specifications
{
    public class RoleSpecs : Specification<Entities.Role>
    {
        private RoleSpecs(Expression<Func<Entities.Role, bool>> predicate)
            : base(predicate)
        { }

        private RoleSpecs(string command, params object[] parameters)
            : base(command, parameters)
        { }

        public static RoleSpecs GetRoles()
        {
            return new RoleSpecs(e => true);
        }
    }
}
