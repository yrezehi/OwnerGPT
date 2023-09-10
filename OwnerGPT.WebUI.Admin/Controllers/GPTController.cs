using Microsoft.AspNetCore.Mvc;
using OwnerGPT.LLM.Models.LLama;
using System.IO;
using System;
using OwnerGPT.Models.Entities.DTO;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("api/[controller]")]
    public class GPTController : BasePartialViewController
    {
        private readonly LLAMAModel StatelessGPT;

        public GPTController(LLAMAModel statelessGPT)
        {
            StatelessGPT = statelessGPT;
        }

        [HttpPost("[action]")]
        public async void Replay(GPTMessageInputDTO messageInput)
        {
            Response.StatusCode = 200;
            Response.ContentType = "text/plain";

            var streamWriter = new StreamWriter(Response.Body);
            
            foreach(var replay in StatelessGPT.Replay(messageInput.Message))
            {
                await streamWriter.WriteLineAsync(replay);
                await streamWriter.FlushAsync();
            }
        }
    }
}
