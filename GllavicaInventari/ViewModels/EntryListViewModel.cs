using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.ViewModels
{
    public class EntryListViewModel
    {
        public string SerialNumber { get; set; }
        public string BillNumber { get; set; }
        public string WareHouseName { get; set; }
        public DateTime DateEntry { get; set; }
        public double TotalValue { get; set; }
        public double TotalValueWithTVH { get; set; }
        public string SupplierName { get; set; }


        public string LoggedInUserFullName { get; set; }
        public string LoggedInUserId { get; set; }

    }
}
