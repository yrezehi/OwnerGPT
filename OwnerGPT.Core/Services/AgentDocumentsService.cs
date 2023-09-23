using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.Models;
using OwnerGPT.Models.Agents;

namespace OwnerGPT.Core.Services
{
    public class AgentDocumentsService : CompositionBaseService<AgentDocument>
    {

        private readonly DocumentService DocumentService;
        
        public AgentDocumentsService(RDBMSServiceBase<AgentDocument> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase, DocumentService documentService) : base(RDBMSServiceBase, PGVServiceBase) {
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

        public async IAsyncEnumerable<int> StreamCreate(IFormFile file)
        {
            await foreach(int progress in DocumentService.StreamPersist(file))
            {
                yield return progress;
            }
        }

        public async Task<string> FetchPreview(int documentId)
        {
            return "";
        }
    }
}
