using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Abstract.Interfaces;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.LLM.PromptEnginnering;
using OwnerGPT.Models.Entities;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Models.Entities.DTO;
using OwnerGPT.Plugins.Manager.Documents.Models;
using OwnerGPT.Plugins.Parsers.PDF;

namespace OwnerGPT.Core.Services
{
    public class AgentDocumentsService : RDBMSServiceBase<AgentDocument>
    {

        private readonly DocumentService DocumentService;

        public AgentDocumentsService(IRDBMSUnitOfWork unitOfWork, DocumentService documentService) : base(unitOfWork){
            DocumentService = documentService;
        }
    }
}
