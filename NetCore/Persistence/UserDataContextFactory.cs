using System;
using System.Collections.Generic;
using System.Linq;
using CrudDatastore;

namespace Persistence
{
    public class UserDataContextFactory : IUserDataContextFactory
    {
        public DataContextBase CreateDataContext()
        {
            return new GenericDataContext(new UserUnitOfWorkInMemory());
        }
    }

    internal class UserUnitOfWorkInMemory : UnitOfWorkBase
    {
        private static readonly IList<Entities.User> _users = new List<Entities.User>();
        private static readonly IList<Entities.UserRole> _userRoles = new List<Entities.UserRole>();
        private static readonly IList<Entities.Role> _roles = new List<Entities.Role>();

        private IDataStore<Entities.Role> Roles()
        {
            return new DataStore<Entities.Role>(
                new DelegateCrudAdapter<Entities.Role>(
                    /* create */
                    (e) =>
                    {
                    },

                    /* update */
                    (e) =>
                    {
                    },

                    /* delete */
                    (e) =>
                    {
                    },

                    /* read */
                    (predicate) =>
                    {
                        return _roles.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        private IDataStore<Entities.User> Users()
        {
            return new DataStore<Entities.User>(
                new DelegateCrudAdapter<Entities.User>(
                    /* create */
                    (e) =>
                    {
                        var nextId = (_users.Any() ? _users.Max(i => i.UserId) : 0) + 1;
                        e.UserId = nextId;

                        _users.Add(new Entities.User
                        {
                            UserId = e.UserId,
                            Username = e.Username,
                            PasswordHash = e.PasswordHash
                        });
                    },

                    /* update */
                    (e) =>
                    {
                        var user = _users.FirstOrDefault(i => i.UserId == e.UserId);
                        if (user != null)
                        {
                            user.PasswordHash = e.PasswordHash;
                        }
                    },

                    /* delete */
                    (e) =>
                    {
                        var user = _users.FirstOrDefault(i => i.UserId == e.UserId);
                        if (user != null)
                        {
                            _users.Remove(user);
                        }
                    },

                    /* read */
                    (predicate) =>
                    {
                        return _users.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        private IDataStore<Entities.UserRole> UserRoles()
        {
            return new DataStore<Entities.UserRole>(
                new DelegateCrudAdapter<Entities.UserRole>(
                    /* create */
                    (e) =>
                    {
                        var nextId = (_userRoles.Any() ? _userRoles.Max(i => i.UserRoleId) : 0) + 1;
                        e.UserRoleId = nextId;

                        _userRoles.Add(new Entities.UserRole
                        {
                            UserRoleId = e.UserRoleId,
                            UserId = e.UserId,
                            Role = e.Role
                        });
                    },

                    /* update */
                    (e) =>
                    {
                        var userRole = _userRoles.FirstOrDefault(i => i.UserRoleId == e.UserRoleId);
                        if (userRole != null)
                        {
                            userRole.Role = e.Role;
                        }
                    },

                    /* delete */
                    (e) =>
                    {
                        var userRole = _userRoles.FirstOrDefault(i => i.UserRoleId == e.UserRoleId);
                        if (userRole != null)
                        {
                            _userRoles.Remove(userRole);
                        }
                    },

                    /* read */
                    (predicate) =>
                    {
                        return _userRoles.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        static UserUnitOfWorkInMemory()
        {
            _roles.Add(new Entities.Role { RoleId = 1, Name = "Admin" });
            _roles.Add(new Entities.Role { RoleId = 2, Name = "User" });
            _roles.Add(new Entities.Role { RoleId = 3, Name = "Guest" });

            _users.Add(new Entities.User { UserId = 1, Username = "Admin", PasswordHash = "vu2RcrhyyP3G1N/QBZk3hMOvrr43jsQ=" });
            _users.Add(new Entities.User { UserId = 2, Username = "User1", PasswordHash = "9AVsRDDZ13M3UjvaXI1GVunHK0TD9q8=" });
            _users.Add(new Entities.User { UserId = 3, Username = "User2", PasswordHash = "xWbauJpHqvpOYTnNDILhGiBV8jFPeQ==" });
            _users.Add(new Entities.User { UserId = 4, Username = "User3", PasswordHash = "kkT7+o5o8EYefBqQ6LD21wVgGV5gHw==" });

            _userRoles.Add(new Entities.UserRole { UserRoleId = 1, UserId = 1, Role = "Admin" });
            _userRoles.Add(new Entities.UserRole { UserRoleId = 2, UserId = 2, Role = "Admin" });
            _userRoles.Add(new Entities.UserRole { UserRoleId = 3, UserId = 2, Role = "User" });
            _userRoles.Add(new Entities.UserRole { UserRoleId = 4, UserId = 3, Role = "User" });
            _userRoles.Add(new Entities.UserRole { UserRoleId = 5, UserId = 4, Role = "Guest" });
        }

        public UserUnitOfWorkInMemory()
        {
            this.Register(Roles());
            this.Register(Users())
                .Map(u => u.Roles, (u, r) => u.UserId == r.UserId);
            this.Register(UserRoles());
        }
    }
}
