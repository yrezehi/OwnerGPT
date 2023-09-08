using OwnerGPT.Controllers.Abstract;
using OwnerGPT.Models.Agents;
using OwnerGPT.Services;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    public class AgentsController : RDBMSBaseController<AgentsService, Agent>
    {
        public AgentsController(AgentsService service) : base(service) { }
    }
}
