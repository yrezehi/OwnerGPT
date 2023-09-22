using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Abstract.Interfaces;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.LLM.PromptEnginnering;
using OwnerGPT.Models.Entities;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Models.Entities.DTO;
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
                    await AgentDocumentsService.Create(agentConfiguration.Agent.Id, attachment);
                    PluginDocument pluginDocument = await PluginDocument.GetPluginDocumentInstance(attachment);

                    string processedFile = PDFParser.Process(pluginDocument.Bytes);

                    if (processedFile != null && processedFile.Length > 0)
                    {
                        var chunkedFiles = SentenceEncoder.ChunkText(processedFile);

                        foreach (var chunk in chunkedFiles)
                        {
                            await PGVServiceBase.Insert(chunk);
                        }

                        if (chunkedFiles.Any())
                        {
                            agentConfiguration.Agent.Instruction += $"\n{Prompts.ANSWER_CONTEXT}\n";
                        }
                    }
                }
            }

            return await RDBMSServiceBase.Update(agentConfiguration.Agent);
        }
    }
}
