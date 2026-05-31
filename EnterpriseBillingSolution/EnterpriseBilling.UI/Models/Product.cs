using System.ComponentModel.DataAnnotations;

namespace EnterpriseBilling.UI.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }
        public string Barcode { get; set; }
        public string NameProduct { get; set; }
        public decimal Cost { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
    }
}
