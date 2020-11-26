using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Specifications;
using WebApplication.Infrastructure;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserDataContextFactory _contextFactory;

        public UserController(IUserDataContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IActionResult Index()
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var users = context
                    .Find(UserSpecs.GetUsers())
                    .ToList();

                return View(new UsersViewModel(users));
            }
        }

        public IActionResult Create()
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var roles = context
                    .Find(RoleSpecs.GetRoles())
                    .ToList();

                return View(new UserCreateViewModel(roles));
            }
        }

        [HttpPost]
        public IActionResult Create(UserCreateEntryModel entryModel)
        {
            if (ModelState.IsValid)
            {
                using (var context = _contextFactory.CreateDataContext())
                {
                    try
                    {
                        var user = new Persistence.Entities.User
                        {
                            Username = entryModel.Username,
                            PasswordHash = ApplicationUserManager.SimpleHash.ComputeHash(entryModel.TempPassword),
                            Roles = entryModel.UserRoles.Select(r => new Persistence.Entities.UserRole { Role = r }).ToList()
                        };

                        context.Add(user);
                        context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return Create();
        }

        public IActionResult Edit(int id)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var user = context.FindSingle(UserSpecs.GetUser(id));
                var roles = context
                    .Find(RoleSpecs.GetRoles())
                    .ToList();

                return View(new UserEditViewModel(user, roles));
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, UserEditEntryModel entryModel)
        {
            if (ModelState.IsValid)
            {
                using (var context = _contextFactory.CreateDataContext())
                {
                    try
                    {
                        var user = context.FindSingle(UserSpecs.GetUser(id));
                        if (entryModel.IsChangePassword)
                        {
                            user.PasswordHash = ApplicationUserManager.SimpleHash.ComputeHash(entryModel.TempPassword);
                        }

                        var userRolesToBeDeleted = user.Roles.Where(r => !entryModel.UserRoles.Contains(r.Role)).ToList();
                        foreach (var userRole in userRolesToBeDeleted)
                        {
                            user.Roles.Remove(userRole);
                        }

                        var userRolesToBeAdded = entryModel.UserRoles.Where(ur => !user.Roles.Any(r => r.Role == ur)).Select(ur => new Persistence.Entities.UserRole { Role = ur }).ToList();
                        foreach (var userRole in userRolesToBeAdded)
                        {
                            user.Roles.Add(userRole);
                        }

                        context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return Edit(id);
        }

        public IActionResult Delete(int id)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var user = context.FindSingle(UserSpecs.GetUser(id));

                if (Request.Method == "POST")
                {
                    try
                    {
                        context.Delete(user);
                        context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }

                var roles = context
                    .Find(RoleSpecs.GetRoles())
                    .ToList();

                return View(new UserDeleteViewModel(user, roles));
            }
        }
    }
}
