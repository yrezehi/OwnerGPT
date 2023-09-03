using OwnerGPT.Models.Agents;
using OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces;

namespace OwnerGPT.Services.Abstract
{
    public class AgentsService : RDBMSServiceBase<AgentBinding>
    {
        public AgentsService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
