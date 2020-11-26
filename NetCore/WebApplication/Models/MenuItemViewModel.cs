using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Models
{
    public class MenuItemViewModel
    {
        private readonly Func<string, bool> _currentUrlComparison;
        private readonly IEnumerable<MenuItemViewModel> _subMenus;

        public MenuItemViewModel()
            : this(string.Empty.Equals, Enumerable.Empty<MenuItemViewModel>())
        { }

        public MenuItemViewModel(IEnumerable<MenuItemViewModel> subMenus)
            : this(string.Empty.Equals, subMenus)
        { }

        public MenuItemViewModel(string currentUrl)
            : this(currentUrl.Equals, Enumerable.Empty<MenuItemViewModel>())
        { }

        public MenuItemViewModel(string currentUrl, IEnumerable<MenuItemViewModel> subMenus)
            : this(currentUrl.Equals, subMenus)
        { }

        public MenuItemViewModel(Func<string, bool> currentUrlComparison)
            : this(currentUrlComparison, Enumerable.Empty<MenuItemViewModel>())
        {  }

        public MenuItemViewModel(Func<string, bool> currentUrlComparison, IEnumerable<MenuItemViewModel> subMenus)
        {
            _currentUrlComparison = currentUrlComparison;
            _subMenus = subMenus;
        }

        public string Text { get; set; }
        public string Url { get; set; }

        public IEnumerable<MenuItemViewModel> SubMenus
        {
            get
            {
                return _subMenus;
            }
        }

        public bool IsActive
        {
            get
            {
                return _currentUrlComparison(Url);
            }
        }   
    }
}
