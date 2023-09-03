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
        public virtual async Task<IActionResult> Update(Agent entityToUpdate)
        {
            return Ok(await Service.Update(entityToUpdate));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Insert([FromBody] Agent entity)
        {
            return Ok(await Service.Insert(entity));
        }
    }
}
