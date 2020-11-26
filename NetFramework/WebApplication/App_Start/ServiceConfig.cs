using System;
using Autofac;
using Persistence;
using WebApplication.Infrastructure;

namespace WebApplication
{
    public class ServiceConfig
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserDataContextFactory>().As<IUserDataContextFactory>();
            builder.RegisterType<ApplicationUserManager>().AsSelf();
        }
    }
}
