using System;
using System.Web.Mvc;
using WebApplication.Infrastructure;

namespace WebApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RedirectDetectorFilter());
        }
    }

    
}
