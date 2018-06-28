using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GllavicaInventari.Models
{
    public class Exit
    {
        public Exit()
        {
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double TotalValue { get; set; }
        public bool HasTVSH { get; set; }
        public double TotalValueWithTVSH { get; set; }

        public DateTime DateExit { get; set; }
        public string LoggedInUserId { get; set; }
        public string LoggedInUserFullName { get; set; }
        public bool IsTransfer { get; set; }
        public bool IsActive { get; set; }


        //relations with other tables
        [Required(ErrorMessage = "Ju lutem, zgjidhni nje magazine")]
        public int WareHouseId { get; set; }
        [ForeignKey("WareHouseId")]
        public Warehouse WareHouse { get; set; }

        [Required(ErrorMessage = "Ju lutem, zgjidhni nje produkt")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Ju duhet, zgjidhni nje furnitor")]
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
    }
}
