using Microsoft.AspNetCore.Mvc;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;
using OwnerGPT.Core.Services;
using OwnerGPT.Models.Abstracts.DTO;
using Microsoft.AspNetCore.Authorization;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class GPTController : BasePartialViewController
    {
        private readonly GPTService StatelessGPT;

        public GPTController(GPTService statelessGPT) =>
            StatelessGPT = statelessGPT;

        [HttpPost("[action]/{agentId}")]
        public async Task StreamReplay([FromBody] GPTMessageInputDTO messageInput, int agentId, CancellationToken cancellationToken)
        {
            var streamWriter = new StreamWriter(Response.Body);
            
            await foreach(var replay in StatelessGPT.StreamReplay(messageInput.Message, agentId, cancellationToken))
            {
                await streamWriter.WriteAsync(replay);
                await streamWriter.FlushAsync();
            }
        }

        [HttpPost("[action]")]
        public IActionResult Replay(GPTMessageInputDTO messageInput, CancellationToken cancellationToken) =>
            Ok(StatelessGPT.Replay(messageInput.Message, cancellationToken));
        
    }
}
