using Microsoft.AspNetCore.Http;
using OwnerGPT.Models.Entities.Agents;

namespace OwnerGPT.Models.Entities.DTO
{
    public class ConfigureAgentDTO
    {
        public Agent? Agent { get; set; }

        public IFormFile[]? Attachments { get; set; }

        public string? URLs { get; set; }
    }
}
