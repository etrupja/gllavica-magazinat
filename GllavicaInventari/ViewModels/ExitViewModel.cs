using GllavicaInventari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.ViewModels
{
    public class ExitsViewModel
    {
        public IEnumerable<Warehouse> Warehouses { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string SerialNumber { get; set; }
    }
}
