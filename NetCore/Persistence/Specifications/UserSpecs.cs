using System;
using System.Linq.Expressions;
using CrudDatastore;

namespace Persistence.Specifications
{
    public class UserSpecs : Specification<Entities.User>
    { 
        private UserSpecs(Expression<Func<Entities.User, bool>> predicate)
            : base(predicate)
        { }

        private UserSpecs(string command, params object[] parameters)
            : base(command, parameters)
        { }

        public static UserSpecs GetUser(int id)
        {
            return new UserSpecs(e => e.UserId == id);
        }

        public static UserSpecs GetUser(string username)
        {
            return new UserSpecs(e => e.Username.ToLower() == username.ToLower());
        }

        public static UserSpecs GetUsers()
        {
            return new UserSpecs(e => true);
        }
    }
}
