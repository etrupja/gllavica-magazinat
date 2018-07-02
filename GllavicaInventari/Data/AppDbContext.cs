using GllavicaInventari.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Exit> Exits { get; set; }
        public DbSet<Transfer> Transfers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transfer>()
                .HasOne(pt => pt.FromWareHouse)
                .WithMany(p => p.FromTransfers)
                .HasForeignKey(pt => pt.FromWareHouseId);

            modelBuilder.Entity<Transfer>()
                .HasOne(pt => pt.ToWareHouse)
                .WithMany(p => p.ToTransfers)
                .HasForeignKey(pt => pt.ToWareHouseId);
        }

        public async Task Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            IdentityResult roleResult;
            //Adding Addmin Role  
            var roleCheck = await roleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            roleCheck = await roleManager.RoleExistsAsync("Manager");
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await roleManager.CreateAsync(new IdentityRole("Manager"));
            }

            //Assign Admin role to the main User here we have given our newly loregistered login id for Admin management  
            ApplicationUser adminAppUser = await userManager.FindByEmailAsync("admin@gllavica.com");
            if (adminAppUser == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@gllavica.com",
                    Email = "admin@gllavica.com",
                    FirstName = "Admin",
                    LastName = "Gllavica",
                    FullName = "Admin Gllavica",
                    EmailConfirmed = true,
                    IsActive = true
                    
                };
                await userManager.CreateAsync(adminUser, "admin2018?!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            //ApplicationUser managerAppUser = await userManager.FindByEmailAsync("menaxher@gllavica.com");
            //if (adminAppUser == null)
            //{
            //    var managerUser = new ApplicationUser
            //    {
            //        UserName = "menaxher@gllavica.com",
            //        Email = "menaxher@gllavica.com",
            //        FirstName = "Menaxher",
            //        LastName = "Gllavica",
            //        FullName = "Menaxher Gllavica",
            //        EmailConfirmed = true,
            //        IsActive = true
            //    };
            //    await userManager.CreateAsync(managerUser, "!menaxher2018?");
            //    await userManager.AddToRoleAsync(managerUser, "Manager");
            //}
        }
    }
}
