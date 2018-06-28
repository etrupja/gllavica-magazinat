using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GllavicaInventari.Models
{
    public class Transfer
    {
        public Transfer()
        {
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string BillNumber { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double TotalValue { get; set; }
        public bool HasTVSH { get; set; }
        public double TotalValueWithTVSH { get; set; }


        [DataType(DataType.Date)]
        public DateTime DateTranfer { get; set; }
        public string LoggedInUserId { get; set; }
        public string LoggedInUserFullName { get; set; }
        public bool IsActive { get; set; }
        
        [ForeignKey("FromWareHouse"), Column(Order = 0)]
        public int? FromWareHouseId { get; set; }
        [ForeignKey("ToWareHouse"), Column(Order = 1)]
        public int? ToWareHouseId { get; set; }
        
        public virtual Warehouse FromWareHouse { get; set; }
        public virtual Warehouse ToWareHouse { get; set; }




        [Required(ErrorMessage = "Ju lutem, zgjidhni nje produkt")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        
    }
}
