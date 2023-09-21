using OwnerGPT.Models.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models.Entities.Agents
{
    [Table("agents")]
    public class Agent : IEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("introduction")]
        public string? Introduction { get; set; }

        [Column("instruction")]
        public string? Instruction { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("creation_date")]
        public DateTime? CreationDate { get; set; }

        public List<string> SearchableProperties()
        {
            return "Name,Description".Split(",").ToList();
        }
    }
}
