using dotenv.net;
using ExternalLogins.Models;
using ExternalLogins.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ExternalLogins
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            DotEnv.Config();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProfileContext>(
                options => options.UseSqlServer(Environment.GetEnvironmentVariable("DEFAULT_CONNECTION_STRING")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ProfileContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                    {
                        facebookOptions.AppId = Environment.GetEnvironmentVariable("FACEBOOK_APP_ID");
                        facebookOptions.AppSecret = Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET");
                    });

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Main");

            services.AddMvc();

            services.AddScoped<IProfileRepository, ProfileRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ProfileContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Main}/{action=Index}/{id?}");
            });

        }
    }
}
