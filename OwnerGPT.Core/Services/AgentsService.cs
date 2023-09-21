using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Abstract.Interfaces;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.DocumentEmbedding.Encoder;
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

        public AgentsService(RDBMSServiceBase<Agent> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase, SentenceEncoder sentenceEncoder) : base(RDBMSServiceBase, PGVServiceBase) {
            SentenceEncoder = sentenceEncoder;
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
                    PluginDocument document = await PluginDocument.Create(attachment);

                    string processedFile = PDFParser.Process(document.Bytes);
                    var chunkedFiles = SentenceEncoder.ChunkText(processedFile);

                    foreach (var chunk in chunkedFiles)
                    {
                        await PGVServiceBase.Insert(chunk);
                    }

                    if (processedFile != null && processedFile.Length > 0)
                    {
                        agentConfiguration.Agent.Instruction += "\n Answer the question based on the context below:\n";
                    }
                }
            }

            return await RDBMSServiceBase.Update(agentConfiguration.Agent);
        }
    }
}
