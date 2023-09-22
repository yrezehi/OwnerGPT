using OwnerGPT.Models.Abstracts.Interfaces;

namespace OwnerGPT.Models.Agents
{
    public class AgentInstruction : IEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Context { get; set; }

        public List<string> SearchableProperties()
        {
            throw new NotImplementedException();
        }
    }
}
