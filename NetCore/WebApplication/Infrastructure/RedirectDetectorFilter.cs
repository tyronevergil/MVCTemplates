using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication.Infrastructure
{
    public class RedirectDetectorFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is RedirectResult || filterContext.Result is RedirectToActionResult || filterContext.Result is RedirectToPageResult || filterContext.Result is RedirectToRouteResult)
            {
                ((Controller)filterContext.Controller).TempData["Redirected"] = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }
}
