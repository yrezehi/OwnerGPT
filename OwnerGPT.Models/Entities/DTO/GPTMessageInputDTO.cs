﻿using Microsoft.AspNetCore.Http;

namespace OwnerGPT.Models.Entities.DTO
{
    public class GPTMessageInputDTO
    {
        public string Message { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
