using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Controllers.Abstract;
using OwnerGPT.Models.Models.Agents;
using OwnerGPT.Models.Models.Bindings;
using OwnerGPT.Services;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    public class AgentsController : RDBMSBaseController<AgentsService, Agent>
    {
        public AgentsController(AgentsService Service) : base(Service) { }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await Service.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm][Bind(AgentBinding.Create)] Agent agent)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            return Ok(await Service.Create(agent));
        }
    }
}
