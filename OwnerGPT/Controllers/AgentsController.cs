using OwnerGPT.Controllers.Abstract;
using OwnerGPT.Models.Agents;
using OwnerGPT.Models.Bindings;
using OwnerGPT.Models.DTO;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Controllers
{
    public class AgentsController : RDBMSBaseController<AgentsService, Agent, AgentDTO>
    {
        public AgentsController(AgentsService Service) : base(Service) { }
    }
}
