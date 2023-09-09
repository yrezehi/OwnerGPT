using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Models.Entities.DTO
{
    public class GPTMessageInputDTO
    {
        public string Message { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
