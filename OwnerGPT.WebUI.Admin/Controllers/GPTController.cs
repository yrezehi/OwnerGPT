using Microsoft.AspNetCore.Mvc;
using OwnerGPT.LLM.Models.LLama;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("api/[controller]")]
    public class GPTController : Controller
    {
        private readonly LLAMAModel StatelessGPT;

        public GPTController(LLAMAModel statelessGPT)
        {
            StatelessGPT = statelessGPT;
        }

        [HttpPost("[action]")]
        public IActionResult Replay([FromBody] string message)
        {
            return Ok(StatelessGPT.Replay(message));
        }
    }
}
