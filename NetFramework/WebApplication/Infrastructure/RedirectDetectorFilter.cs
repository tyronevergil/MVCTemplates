using System;
using System.Web.Mvc;

namespace WebApplication.Infrastructure
{
    public class RedirectDetectorFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is RedirectResult || filterContext.Result is RedirectToRouteResult)
            {
                filterContext.Controller.TempData["Redirected"] = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}
