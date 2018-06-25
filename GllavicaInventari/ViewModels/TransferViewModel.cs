using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.ViewModels
{
    public class TransferViewModel
    {
        public string TransferCode { get; set; }

        [Required(ErrorMessage = "Ju lutem, percaktoni magazinen nga ku do dale malli")]
        public int FromWareHouseId { get; set; }

        [Required(ErrorMessage = "Ju lutem, percaktoni produktin")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Ju lutem, percaktoni sasine e produktit")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Ju lutem, percaktoni magazinen ku do te shkoje malli")]
        public int ToWareHouseId { get; set; }
    }
}
