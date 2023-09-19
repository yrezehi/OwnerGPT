using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Models.Entities.Bindings;
using OwnerGPT.Models.Entities.DTO;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    public class AgentsController : RDBMSBaseController<AgentsService, Agent>
    {
        public AgentsController(AgentsService Service) : base(Service) { }

        public async Task<IActionResult> Index() =>
            View(await Service.GetAll());

        [HttpGet("[action]/{agentId}")]
        public async Task<IActionResult> Configure(int agentId) =>
            View(await Service.FindById(agentId));

        [HttpGet("[action]/{agentId}")]
        public async Task<IActionResult> Chat(int agentId) =>
            View(await Service.FindById(agentId));

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm][Bind(AgentBinding.Create)] Agent agent)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            await Service.Create(agent);

            return RedirectToAction("Index");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromForm] ConfigureAgentDTO agentConfiguration)
        {
            if (!ModelState.IsValid || agentConfiguration.Agent == null)
            {
                return BadRequest(ModelState.ValidationState);
            }

            await Service.UpdateConfiguration(agentConfiguration);

            return RedirectToAction("Index");
        }

    }
}
