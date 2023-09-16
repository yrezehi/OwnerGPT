using OwnerGPT.Models.Entities.Interfaces;

namespace OwnerGPT.Models.Entities.Agents
{
    public class AgentInstruction : IEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Context { get; set; }
    }
}
