using GllavicaInventari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.ViewModels
{
    public class EntryViewModel
    {
        public IEnumerable<Supplier> Suppliers { get; set; }
        public IEnumerable<Warehouse> Warehouses { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string SerialNumber { get; set; }
        public string BillNumber { get; set; }
        public bool HasTVSH { get; set; }
    }
}
