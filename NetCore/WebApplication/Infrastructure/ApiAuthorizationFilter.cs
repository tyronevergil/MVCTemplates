using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication.Infrastructure
{
    public class ApiAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var url = context.HttpContext.Request.Path.Value;
            if (url.StartsWith("/api/"))
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
