using GllavicaInventari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.ViewModels
{
    public class WarehouseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Entry> Entries { get; set; }
        public List<Exit> Exits { get; set; }
        public List<Stock> Stocks { get; set; }
    }
}
