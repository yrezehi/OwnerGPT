using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Controllers.Abstract;
using OwnerGPT.Models.Agents;
using OwnerGPT.Models.Bindings;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Controllers
{
    public class AgentsController : RDBMSBaseController<AgentsService, Agent>
    {
        public AgentsController(AgentsService Service) : base(Service) { }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody][Bind(AgentBinding.Edit)] Agent entityToUpdate)
        {
            return Ok(await Service.Update(entityToUpdate));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody][Bind(AgentBinding.Create)] Agent entity)
        {
            return Ok(await Service.Create(entity));
        }
    }
}
