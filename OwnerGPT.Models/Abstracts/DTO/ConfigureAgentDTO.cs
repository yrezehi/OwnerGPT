using Microsoft.AspNetCore.Http;
using OwnerGPT.Models.Agents;

namespace OwnerGPT.Models.Abstracts.DTO
{
    public class ConfigureAgentDTO
    {
        public Agent? Agent { get; set; }

        public IFormFile[]? Attachments { get; set; }

        public string? URLs { get; set; }
    }
}
