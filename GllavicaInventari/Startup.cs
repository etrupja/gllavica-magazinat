using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GllavicaInventari.Data;
using GllavicaInventari.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            //string cString = @"Data Source=ERVIS-PC\SQLEXPRESS;Initial Catalog=GllavicaInventory;Integrated Security=True";
            string cString = @"Server=gllavica.database.windows.net;Initial Catalog=GllavicaInventory;Persist Security Info=False;User ID=gllavica;Password=Inventari2018?!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(cString));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                //password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                //user settings
                options.User.RequireUniqueEmail = false;

                //lockout settings
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(5);
                options.LoginPath = "/Account/Login";
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services, AppDbContext context)
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
           

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "entryDetails",
                //    template: "{controller=Entries}/{action=BillDetails}/{serialNumber}");
                //routes.MapRoute(
                //    name: "stocksfilter",
                //    template: "{controller=Stock}/{action=Details}/{id}/{dateStart}/{dateEnd}");
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
