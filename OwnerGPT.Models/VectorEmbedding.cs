using OwnerGPT.Models.Abstracts.Interfaces;
using Pgvector;

namespace OwnerGPT.Models
{
    public class VectorEmbedding : IEntity
    {
        public int Id { get; set; }
        public Vector? Embedding { get; set; }
        public string? Context { get; set; }
        public int ChunkId { get; set; }

        public List<string> SearchableProperties()
        {
            throw new NotImplementedException();
        }
    }
}
