using Microsoft.AspNetCore.Http;

namespace OwnerGPT.Models.Abstracts.DTO
{
    public class GPTMessageInputDTO
    {
        public string Message { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
