using System.ComponentModel.DataAnnotations.Schema;
using OwnerGPT.Models.Interfaces;
using Pgvector;

namespace OwnerGPT.Models
{
    public class VectorEmbedding : IEntity
    {
        public int Id { get; set; }

        public Vector Embedding { get; set; } 
    }
}
