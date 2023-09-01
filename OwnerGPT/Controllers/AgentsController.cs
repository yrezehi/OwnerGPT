using OwnerGPT.Controllers.Abstract;
using OwnerGPT.Models.Agents;
using OwnerGPT.Models.DTO;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Controllers
{
    [Route("api/[controller]")]
    public class AgentsController : RDBMSBaseController<AgentsService, Agent, AgentDTO>
    {
        public AgentsController(AgentsService Service) : base(Service) { }
    }
}
