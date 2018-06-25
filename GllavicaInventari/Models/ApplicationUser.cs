using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
            IsActive = true;
            EmailConfirmed = true;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public List<Warehouse> Warehouses { get; set; }
    }
}
