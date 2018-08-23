using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GllavicaInventari.Data;
using GllavicaInventari.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GllavicaInventari
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //services.Configure<IdentityOptions>(options =>
            //{
            //    //password settings
            //    options.Password.RequireDigit = false;
            //    options.Password.RequiredLength = 8;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireNonAlphanumeric = false;

            //    //user settings
            //    options.User.RequireUniqueEmail = false;

            //    //lockout settings
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            //});
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.Expiration = TimeSpan.FromDays(5);
            //    options.LoginPath = "/Account/Login";
            //});
            //services.AddDistributedMemoryCache();
            //services.AddSession();
            //services.AddDataProtection().DisableAutomaticKeyGeneration();

            //services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IServiceProvider services, 
            AppDbContext context)
        {
            try
            {
                context.Database.Migrate();
                context.Seed(services).Wait();
            }
            catch (Exception)
            {
                if (env.IsDevelopment())
                {
                    app.UseBrowserLink();
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Error/Index");
                }
            }
           

            //app.UseStaticFiles();
            //app.UseAuthentication();
            //app.UseCookiePolicy();
            //app.UseSession();

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("~/Logs/mylog-{Date}.txt");

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "entryDetails",
                //    template: "{controller=Entries}/{action=BillDetails}/{serialNumber}");
                routes.MapRoute(
                   name: "entriesfilter",
                   template: "{controller=Entries}/{action=Index}/{dateStart}/{dateEnd}");
                routes.MapRoute(
                   name: "exitsfilter",
                   template: "{controller=Exits}/{action=Index}/{dateStart}/{dateEnd}");
                routes.MapRoute(
                    name: "stocksfilter",
                    template: "{controller=Stock}/{action=Details}/{id}/{dateStart}/{dateEnd}");
                //routes.MapRoute(
                //   name: "stocksdetails",
                //   template: "{controller=Stock}/{action=Details}/{id}");
                routes.MapRoute(
                    name: "stockindex",
                    template: "{controller=Stock}/{action=Index}/{id?}");
               
            });
        }
    }
}
