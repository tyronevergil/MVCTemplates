using System;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Autofac;
using Autofac.Integration.Mvc;

namespace WebApplication
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            ServiceConfig.RegisterServices(builder);

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            FormsAuthenticationTicket authTicket = GetFormsAuthenticationTicket();

            if (authTicket != null)
            {
                string[] roles = authTicket.UserData.Split(new Char[] { ',' });
                var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);

                Context.User = userPrincipal;
            }
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            FormsAuthenticationTicket authTicket = GetFormsAuthenticationTicket();

            if (authTicket != null)
            {
                if ((authTicket.Expired && Request.IsAuthenticated) ||
                    (!authTicket.Expired && !authTicket.IsPersistent && ((Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState) && Session["AuthSync"] == null)))
                {
                    FormsAuthentication.SignOut();
                    Context.Response.Redirect("/", true);
                    return;
                }
            }
        }

        private FormsAuthenticationTicket GetFormsAuthenticationTicket()
        {
            FormsAuthenticationTicket authTicket = default(FormsAuthenticationTicket);

            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch (Exception)
                { }
            }

            return authTicket;
        }
    }
}
