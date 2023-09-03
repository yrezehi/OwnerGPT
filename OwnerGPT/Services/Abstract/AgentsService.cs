using OwnerGPT.Models.Agents;
using OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces;

namespace OwnerGPT.Services.Abstract
{
    public class AgentsService : RDBMSServiceBase<Agent>
    {
        public AgentsService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
