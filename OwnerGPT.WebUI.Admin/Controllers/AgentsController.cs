using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services;
using OwnerGPT.Models.Abstracts.Bindings;
using OwnerGPT.Models.Agents;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AgentsController : RDBMSBaseController<AgentsService, Agent>
    {
        public AgentsController(AgentsService Service) : base(Service) { }

        public async Task<IActionResult> Index(int? page, string property = "name", string value = "") =>
            View(await Service.RDBMSServiceBase.SearchByProperty(property, value, page));

        [HttpGet("[action]/{agentId}")]
        public async Task<IActionResult> Configure(int agentId) =>
            View(await Service.RDBMSServiceBase.FindById(agentId));

        [HttpGet("[action]/{agentId}")]
        public IActionResult KnowledgeBase() =>
            View();

        [HttpGet("[action]/{agentId}")]
        public async Task<IActionResult> Chat(int agentId) =>
            View(await Service.RDBMSServiceBase.FindById(agentId));

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm][Bind(AgentBinding.Create)] Agent agent)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            await Service.RDBMSServiceBase.Create(agent);

            return RedirectToAction("Index");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromForm][Bind(AgentBinding.Update)] Agent agent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            await Service.UpdateConfiguration(agent);

            return RedirectToAction("Index");
        }

    }
}
