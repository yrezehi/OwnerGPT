using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services;
using OwnerGPT.Models.Abstracts.Bindings;
using OwnerGPT.Models.Abstracts.DTO;
using OwnerGPT.Models.Agents;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    public class AgentDocumentsController : RDBMSBaseController<AgentDocumentsService, AgentDocument>
    {
        public AgentDocumentsController(AgentDocumentsService Service) : base(Service) { }


    }
}
