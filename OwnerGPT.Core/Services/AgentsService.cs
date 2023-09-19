using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.Core.Utilities.Extenstions;
using OwnerGPT.DB.Repositores.PGVDB;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.Models.Entities;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Models.Entities.DTO;
using OwnerGPT.Plugins.Parsers.PDF;

namespace OwnerGPT.Core.Services
{
    public class AgentsService : ICompositionBaseService<Agent>
    {

        private readonly SentenceEncoder SentenceEncoder;

        public AgentsService(RDBMSServiceBase<Agent> RDBMSServiceBase, PGVServiceBase<Agent> PGVServiceBase, SentenceEncoder sentenceEncoder) : base(RDBMSServiceBase, PGVServiceBase) {
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
                    byte[] fileBytes = await attachment.GetBytes();

                    if (fileBytes.Length == 0)
                    {
                        throw new Exception("Attachment is not valid!");
                    }

                    string processedFile = PDFParser.Process(fileBytes);
                    var chunkedFiles = SentenceEncoder.ChunkText(processedFile);

                    foreach (var chunk in chunkedFiles)
                    {
                        await PGVServiceBase.Insert(chunk);
                    }


                    if (processedFile != null && processedFile.Length > 0)
                    {
                        agentConfiguration.Agent.Instruction += "\n Answer using below information if possible:\n" + processedFile;
                    }
                }
            }

            return await RDBMSServiceBase.Update(agentConfiguration.Agent);
        }
    }
}
