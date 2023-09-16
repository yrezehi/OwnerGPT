using Microsoft.AspNetCore.Mvc;
using OwnerGPT.LLM.Models.LLama;
using System.IO;
using System;
using OwnerGPT.Models.Entities.DTO;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;
using OwnerGPT.Core.Services;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("api/[controller]")]
    public class GPTController : BasePartialViewController
    {
        private readonly GPTService StatelessGPT;

        public GPTController(GPTService statelessGPT)
        {
            StatelessGPT = statelessGPT;
        }

        [HttpPost("[action]/{agentId}")]
        public async void StreamReplay([FromBody] GPTMessageInputDTO messageInput, int agentId, CancellationToken cancellationToken)
        {
            var streamWriter = new StreamWriter(Response.Body);
            
            await foreach(var replay in StatelessGPT.StreamReplay(messageInput.Message, agentId, cancellationToken))
            {
                await streamWriter.WriteAsync(replay);
                await streamWriter.FlushAsync();
            }
        }

        [HttpPost("[action]")]
        public IActionResult Replay(GPTMessageInputDTO messageInput, CancellationToken cancellationToken)
        {
            return Ok(StatelessGPT.Replay(messageInput.Message, cancellationToken));
        }
    }
}
