namespace OwnerGPT.Models.Agent
{
    public class AgentConversationHistory
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int Message { get; set; }

        public int AgentId { get; set; }
    }
}
