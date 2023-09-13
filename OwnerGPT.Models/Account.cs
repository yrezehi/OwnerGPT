using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models
{
    public class Account
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("last_login")]
        public DateTime? LastLogin { get; set; }
    }
}
