using Microsoft.AspNetCore.Mvc;
using OwnerGPT.LLM.Services;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("api/[controller]")]
    public class GPTController : Controller
    {
        private readonly StatelessGPTService StatelessGPTService;

        public GPTController(StatelessGPTService statelessGPTService)
        {
            StatelessGPTService = statelessGPTService;
        }

        [HttpPost("[action]")]
        public IActionResult Replay([FromBody] string message)
        {
            return Ok(StatelessGPTService.Replay(message));
        }
    }
}
