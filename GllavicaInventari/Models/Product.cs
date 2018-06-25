using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.Models
{
    //tabela e perdorur per mallrat
    public class Product
    {
        public Product()
        {
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Mungon emrtimi artikullit")]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasTVSH { get; set; }
        [Required(ErrorMessage = "Mungon njesia")]
        public string Unit { get; set; }
        public DateTime DateCreated { get; set; }
        public string LoggedInUserFullName { get; set; }
        public bool IsActive { get; set; }


        //Relations
        public List<Entry> Entries { get; set; }
        public List<Exit> Exits { get; set; }
        public List<Transfer> Transfers { get; set; }
    }
}
