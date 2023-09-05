using OwnerGPT.Models.Agents;
using OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Services.Abstract;

namespace OwnerGPT.Services
{
    public class AgentsService : RDBMSServiceBase<Agent>
    {
        public AgentsService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
