using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models.Entities
{
    [Table("accounts")]
    public class Account
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("last_login")]
        public DateTime? LastSignin { get; set; }

        [Column("active")]
        public bool Active { get; set; }
    }
}
