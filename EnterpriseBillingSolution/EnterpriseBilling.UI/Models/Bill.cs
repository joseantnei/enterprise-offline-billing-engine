using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseBilling.UI.Models
{
    public class Bill
    {
        [Key]
        public int IdBill { get; set; }
        public string BillNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal TotalPay { get; set; }

        //Dependencias Cuustomers Id, User IdUser. 
        [ForeignKey("Customer")]
        public int Id {  get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }
        public virtual User User { get; set; }

        // 3. Relación 1 a Muchos: Una factura tiene MUCHOS detalles
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
