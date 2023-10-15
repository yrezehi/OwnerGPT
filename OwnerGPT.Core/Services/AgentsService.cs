using Microsoft.EntityFrameworkCore;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.Models;
using OwnerGPT.Models.Agents;

namespace OwnerGPT.Core.Services
{
    public class AgentsService : CompositionBaseService<Agent>
    {

        public AgentsService(RDBMSServiceBase<Agent> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase) : base(RDBMSServiceBase, PGVServiceBase) { }

        public async Task<Agent> UpdateConfiguration(Agent agent) =>
            await RDBMSServiceBase.Update(agent);

        public async Task<Agent> CreateDefault(Agent agent) =>
            await this.RDBMSServiceBase.Create(SetDefaultConfiguration(agent));

        private Agent SetDefaultConfiguration(Agent agent)
        {
            (
                agent.Introduction,
                agent.Instruction
            ) = (
                $"Hi There! I'm your personal assistant {agent.Name}, ask me anything!",
                $"You are an AI assistant called {agent.Name} that helps people find information and responds in rhyme. If the user asks you a question you don't know the answer to, say so."
            );

            return agent;
        }

    }
}
