using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseBilling.UI.Models
{
    public class BillDetail
    {
        [Key]
        public int IdBillDetail { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal PercentTaxes { get; set; }

        //Dependencias Bill IdBill, Product IdProduct.
        [ForeignKey(nameof(IdBill))]
        public virtual Bill Bill { get; set; }
        public int IdBill {  get; set; }
        
        [ForeignKey(nameof(IdProduct))]
        public virtual Product Product { get; set; }
        public int IdProduct { get; set; }
    }
}
