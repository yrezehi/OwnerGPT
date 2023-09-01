using OwnerGPT.Controllers.Abstract;
using OwnerGPT.Models.Agents;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Controllers
{
    public class AgentsController : BaseController<AgentsService, Agent, GenreDTO>
    {
        public GenresController(GenresService Service) : base(Service) { }
    }
}
