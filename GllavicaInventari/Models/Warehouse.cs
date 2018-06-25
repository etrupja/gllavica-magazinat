using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.Models
{
    //tabela per magazinat
    public class Warehouse
    {
        public Warehouse()
        {
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }


        //Relations
        public List<Entry> Entries { get; set; }
        public List<Exit> Exits { get; set; }
        public List<Transfer> ToTransfers { get; set; }
        public List<Transfer> FromTransfers { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
