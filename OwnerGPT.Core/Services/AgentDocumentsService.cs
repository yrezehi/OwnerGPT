using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models;
using OwnerGPT.Models.Agents;

namespace OwnerGPT.Core.Services
{
    public class AgentDocumentsService : RDBMSServiceBase<AgentDocument>
    {
        private readonly DocumentService DocumentService;

        public AgentDocumentsService(IRDBMSUnitOfWork unitOfWork, DocumentService documentService) : base(unitOfWork){
            DocumentService = documentService;
        }

        public async Task<AgentDocument> Create(int agnetId, IFormFile file)
        {
            AgentDocument agentDocument = new AgentDocument();
            Document document = await DocumentService.Persist(file);

            agentDocument.AgentId = agnetId;
            agentDocument.DocumentId = document.Id;

            return await this.Create(agentDocument);
        }
    }
}
