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
using StackExchange.Redis;

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
            
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddDataProtection()
                .SetApplicationName("gllavica-inventari")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"~/keys/"));
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

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("~/Logs/mylog-{Date}.txt");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "entriesfilter",
                   template: "{controller=Entries}/{action=Index}/{dateStart}/{dateEnd}");
                routes.MapRoute(
                   name: "exitsfilter",
                   template: "{controller=Exits}/{action=Index}/{dateStart}/{dateEnd}");
                routes.MapRoute(
                    name: "stocksfilter",
                    template: "{controller=Stock}/{action=Details}/{id}/{dateStart}/{dateEnd}");
                routes.MapRoute(
                    name: "stockindex",
                    template: "{controller=Stock}/{action=Index}/{id?}");
               
            });
        }
    }
}
