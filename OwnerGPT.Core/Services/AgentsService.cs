using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Models.Entities.DTO;

namespace OwnerGPT.Core.Services
{
    public class AgentsService : RDBMSServiceBase<Agent>
    {
        public AgentsService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Agent> UpdateConfiguration(ConfigureAgentDTO agentConfiguration)
        {
             if(agentConfiguration.Agent == null)
            {
                throw new Exception("Agent not found in configuration");
            }

             PDFPlugin

            return await this.Update(agentConfiguration.Agent);
        }
    }
}
