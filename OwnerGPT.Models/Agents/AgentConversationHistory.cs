using OwnerGPT.Models.Abstracts.Interfaces;

namespace OwnerGPT.Models.Agents
{
    public class AgentConversationHistory : IEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string? Message { get; set; }
        public int AgentId { get; set; }

        public List<string> SearchableProperties()
        {
            throw new NotImplementedException();
        }
    }
}
