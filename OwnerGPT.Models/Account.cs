using OwnerGPT.Models.Abstracts.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models
{
    [Table("accounts")]
    public class Account : IEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("last_login")]
        public DateTime? LastSignin { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("password")]
        public string Password { get; set; }

        public List<string> SearchableProperties()
        {
            throw new NotImplementedException();
        }
    }
}
