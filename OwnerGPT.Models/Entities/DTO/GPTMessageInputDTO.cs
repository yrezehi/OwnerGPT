using Microsoft.AspNetCore.Http;

namespace OwnerGPT.Models.Entities.DTO
{
    public class GPTMessageInputDTO
    {
        public string Message { get; set; } = "hi";
        public IFormFile Attachment { get; set; }
    }
}
