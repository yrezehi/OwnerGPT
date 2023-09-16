using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models.Entities.Agents
{
    [Table("agents")]
    public class Agent
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }
    }
}
