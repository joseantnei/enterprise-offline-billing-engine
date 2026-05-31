using System.ComponentModel.DataAnnotations;

namespace EnterpriseBilling.UI.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }
    }
}
