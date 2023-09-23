using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.Models;
using OwnerGPT.Models.Agents;

namespace OwnerGPT.Core.Services
{
    public class AgentDocumentsService : CompositionBaseService<AgentDocument>
    {

        private readonly DocumentService DocumentService;
        
        public AgentDocumentsService(RDBMSServiceBase<Agent> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase, DocumentService documentService) : base(RDBMSServiceBase, PGVServiceBase) {
            DocumentService = documentService;
        }

        public async Task<AgentDocument> Create(int agnetId, IFormFile file)
        {
            AgentDocument agentDocument = new AgentDocument();
            Document document = await DocumentService.Persist(file);

            agentDocument.AgentId = agnetId;
            agentDocument.DocumentId = document.Id;

            return await RDBMSServiceBase.Create(agentDocument);
        }
    }
}
