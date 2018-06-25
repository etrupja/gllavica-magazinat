using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.Models
{
    public class Stock
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string Unit { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double TotalValue { get; set; }
    }
}
