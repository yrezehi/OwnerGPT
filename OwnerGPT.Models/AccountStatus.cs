using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models
{
    [Table("account_status")]
    public class AccountStatus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
