using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models.Agents
{
    [Table("agent_document")]
    public class AgentDocument
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("agent_id")]
        public int AgentId { get; set; }

        [Column("document_id")]
        [ForeignKey("Document")]
        public int DocumentId { get; set; }

        public virtual Document Document { get; set; }
    }
}
