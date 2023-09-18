using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Utilities.Extenstions;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Models.Entities.DTO;
using OwnerGPT.Plugins.Parsers.PDF;

namespace OwnerGPT.Core.Services
{
    public class AgentsService : RDBMSServiceBase<Agent>
    {
        public AgentsService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Agent> UpdateConfiguration(ConfigureAgentDTO agentConfiguration)
        {
             if(agentConfiguration.Agent == null)
            {
                throw new Exception("Agent not found in configuration!");
            }

            if(agentConfiguration.Attachments != null && agentConfiguration.Attachments.Length > 0)
            {
                IFormFile attachment = agentConfiguration.Attachments.First();

                if(attachment != null)
                {
                    byte[] fileBytes = await attachment.GetBytes();

                    if (fileBytes.Length == 0)
                    {
                        throw new Exception("Attachment is not valid!");
                    }

                    string processedFile = PDFParser.Process(fileBytes);
                }
            }

            return await this.Update(agentConfiguration.Agent);
        }
    }
}
