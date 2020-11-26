using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ComponentController : Controller
    {
        public ActionResult Navigation(RouteValueDictionary routeValues)
        {
            var currentUrl = Url.Action(routeValues["action"].ToString(), routeValues["controller"].ToString());

            var userMenu = new MenuItemViewModel { Text = "Sign In", Url = Url.Action("Login", "Account") };
            if (User.Identity.IsAuthenticated)
            {
                var userSubMenus = new List<MenuItemViewModel>();
                userSubMenus.Add(new MenuItemViewModel(currentUrl) { Text = "Change Password", Url = Url.Action("Password", "Account") });
                if (User.IsInRole("Admin"))
                {
                    userSubMenus.Add(new MenuItemViewModel(currentUrl.StartsWith) { Text = "User Management", Url = Url.Action("Index", "User") });
                }

                userMenu = new MenuItemViewModel(userSubMenus) { Text = "Sign Out", Url = Url.Action("Logout", "Account") };
            }

            var appMenus = new List<MenuItemViewModel>();
            appMenus.Add(new MenuItemViewModel(currentUrl) { Text = "Home", Url = Url.Action("Index", "Home") });
            if (User.IsInRole("Admin"))
            {
                appMenus.Add(new MenuItemViewModel(currentUrl) { Text = "Admin", Url = Url.Action("Admin", "Home") });
            }

            return PartialView(new NavigationViewModel(User.Identity.IsAuthenticated, userMenu, appMenus));
        }
    }
}