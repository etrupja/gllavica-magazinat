using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.Models
{
    //tabela per furnitorin
    public class Supplier
    {
        public Supplier()
        {
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string LoggedInUserId { get; set; }
        public bool IsActive { get; set; }

        //Relations
        public List<Entry> Entries { get; set; }
        public List<Exit> Exits { get; set; }
    }
}
