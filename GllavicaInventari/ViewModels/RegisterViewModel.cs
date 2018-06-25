using GllavicaInventari.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.ViewModels
{
    public class RegisterViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Mungon emri")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Mungon mbiemri")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Mungon emaili")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mungon roli")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Mungon passwordi")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mungon passwordi")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passworded nuk jane njelloj")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public List<Warehouse> Warehouses { get; set; }
    }
}
