using OwnerGPT.Models.Entities.Interfaces;
using Pgvector;

namespace OwnerGPT.Models.Entities
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
