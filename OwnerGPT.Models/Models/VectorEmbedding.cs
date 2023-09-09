using System.ComponentModel.DataAnnotations.Schema;
using OwnerGPT.Models.Models.Interfaces;
using Pgvector;

namespace OwnerGPT.Models.Models
{
    public class VectorEmbedding : IEntity
    {
        public int Id { get; set; }

        public Vector Embedding { get; set; }

        public string Context { get; set; }
    }
}
