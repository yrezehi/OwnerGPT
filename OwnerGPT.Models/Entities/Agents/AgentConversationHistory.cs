using OwnerGPT.Models.Entities.Interfaces;

namespace OwnerGPT.Models.Entities.Agents
{
    public class AgentConversationHistory : IEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Message { get; set; }
        public int AgentId { get; set; }
    }
}
