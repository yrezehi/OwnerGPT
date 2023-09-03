using OwnerGPT.Controllers.Abstract;
using OwnerGPT.Models.Agents;
using OwnerGPT.Models.DTO;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Controllers
{
    public class AgentsController : RDBMSBaseController<AgentsService, AgentBinding, AgentDTO>
    {
        public AgentsController(AgentsService Service) : base(Service) { }
    }
}
