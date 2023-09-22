﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwnerGPT.Models.Entities.Agents
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
        public string DocumentId { get; set; }
    }
}
