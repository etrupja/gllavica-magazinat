using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.ViewModels
{
    public class StockViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }
        public string ManagerFullName { get; set; }
        //public string Address { get; set; }

        public string StockYear { get; set; }
        //public int EntriesCount { get; set; }
        //public int ExitsCount { get; set; }
    }
}
