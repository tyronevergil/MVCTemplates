using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class NavigationViewModel
    {
        public NavigationViewModel(bool isAuthenticated, MenuItemViewModel userMenu, IEnumerable<MenuItemViewModel> appMenus)
        {
            IsAuthenticated = isAuthenticated;
            UserMenu = userMenu;
            AppMenus = appMenus;
        }

        public bool IsAuthenticated { get; private set; }
        public MenuItemViewModel UserMenu { get; private set; }
        public IEnumerable<MenuItemViewModel> AppMenus { get; private set; }
    }
}
