using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.ViewModels
{
    public class DatatableResultsViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string unit { get; set; }
        public string loggedInUserFullName { get; set; }
        public string surname { get; set; }
        public string company { get; set; }
        public string number { get; set; }
        public int minquantity { get; set; }
        public string nipt { get; set; }
        public string email { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string address { get; set; }
        public DateTime datecreated { get; set; }
        public bool status { get; set; }
        public string actions { get; set; }
        public double bprice { get; set; }
        public double sprice { get; set; }
        public string category { get; set; }
    }
}
