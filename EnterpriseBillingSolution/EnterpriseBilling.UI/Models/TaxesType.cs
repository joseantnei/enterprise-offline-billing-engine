using System.ComponentModel.DataAnnotations;

namespace EnterpriseBilling.UI.Models
{
    public class TaxesType
    {
        [Key]
        public int IdTaxesType { get; set; }
        public string NameTaxes { get; set; }
        public decimal Percent { get; set; }
    }
}
