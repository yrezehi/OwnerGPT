using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.Models;
using OwnerGPT.Models.Abstracts.DTO;
using OwnerGPT.Models.Agents;
using OwnerGPT.Plugins.Manager.Documents.Models;
using OwnerGPT.Plugins.Parsers.PDF;

namespace OwnerGPT.Core.Services
{
    public class AgentsService : CompositionBaseService<Agent>
    {

        private readonly SentenceEncoder SentenceEncoder;
        private readonly AgentDocumentsService AgentDocumentsService;

        public AgentsService(RDBMSServiceBase<Agent> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase, SentenceEncoder sentenceEncoder, AgentDocumentsService agentDocumentsService) : base(RDBMSServiceBase, PGVServiceBase)
        {
            SentenceEncoder = sentenceEncoder;
            AgentDocumentsService = agentDocumentsService;
        }

        public async Task<Agent> UpdateConfiguration(Agent agent) =>
            await RDBMSServiceBase.Update(agent);
    }
}
