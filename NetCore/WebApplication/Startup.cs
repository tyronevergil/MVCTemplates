using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Persistence;
using SimpleBus;
using WebApplication.Infrastructure;

namespace WebApplication
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
                    {
                        var sessionTimeout = 60;
                        int.TryParse(_configuration["Timeout:Session"], out sessionTimeout);

                        options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
                    });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.AccessDeniedPath = new PathString("/Account/Login");
                        options.SlidingExpiration = true;

                        options.Events.OnSignedIn = context =>
                        {
                            context.HttpContext.Session.SetInt32("AuthSync", 1);
                            return Task.CompletedTask;
                        };

                        options.Events.OnValidatePrincipal = context =>
                        {
                            var authProperties = context.Properties;

                            if (!authProperties.IsPersistent && authProperties.ExpiresUtc > DateTimeOffset.UtcNow)
                            {
                                var authSync = context.HttpContext.Session.GetInt32("AuthSync");
                                if (authSync == null)
                                {
                                    context.RejectPrincipal();
                                    return context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                                }
                            }

                            return Task.CompletedTask;
                        };
                    });

            services.AddMvc(options =>
                    {
                        options.Filters.Add(typeof(ApiAuthorizationFilter));
                        options.Filters.Add(typeof(RedirectDetectorFilter));
                    })
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    });

            services.AddSignalR();

            services.AddSingleton<IUserDataContextFactory, UserDataContextFactory>();
            services.AddSingleton<IEventDataContextFactory, EventDataContextFactory>();
            services.AddSingleton<IEventStore, EventStore>();
            services.AddSingleton<IServiceBus, ServiceBus>();
            services.AddScoped<ApplicationUserManager>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<DataHub>("/datahub");
            });
        }

    }
}
