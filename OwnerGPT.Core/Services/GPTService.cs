using OwnerGPT.LLM.Models.LLama;
using OwnerGPT.LLM.PromptEnginnering;
using System.Text;

namespace OwnerGPT.Core.Services
{
    public class GPTService
    {

        public readonly LLamaModel LLamaModel;

        public readonly AgentsService AgentsService;

        public GPTService(AgentsService agentsService, LLamaModel llamaModel) { 
            LLamaModel = llamaModel;
            AgentsService = agentsService;
        }

        public async IAsyncEnumerable<string> StreamReplay(string prompt, int agentId, CancellationToken cancellationToken)
        {
            var promptToExecute = await this.ConstructPrompt(prompt, agentId);

            foreach (var response in LLamaModel.Executor.Infer(promptToExecute, LLamaModel.InferenceParams, cancellationToken))
            {
                yield return response;
            }
        }

        // TODO: remove prompt logic from replay functionallity
        public async Task<string> ConstructPrompt(string prompt, int agentId)
        {
            var agent = await AgentsService.RDBMSServiceBase.NullableFindById(agentId);

            var promptToExecute = "";

            // TODO: can instruction be null but does not have an context?
            if (!string.IsNullOrEmpty(agent?.Instruction))
            {

                // TODO: nearest neighbor needs get single nearest neighbor
                // TODO: remove composition man.
                var retreviedContext = await AgentsService.PGVServiceBase.NearestNeighbor(prompt);

                // there is relative context
                if (retreviedContext.Count() > 0)
                {
                    var contextEmbedding = retreviedContext.First();

                    // TODO: move below string to prompt enginner module
                    promptToExecute += "Answer the Question based on the Context below. Keep the answer short and concise. Respond \"Unsure about answer\" if you don't know.\n\n";
                    promptToExecute += "Question: " + prompt + "\n\n";
                    promptToExecute += "Context: " + contextEmbedding.Context;
                }
                else
                { // there is no relative context
                    promptToExecute += agent?.Instruction;
                }

                // TODO: change inference-params's anti-prompt to be "Answer: " 
                // TODO: create prompt manager put answer suffix
                promptToExecute += "\n\nAnswer: ";

            }
            else
            { // setup default instruction which should be defined TODO: globally? make it generic assistant, make bob you own..
                promptToExecute = PromptContexts.BOB_ASSISTANT;
            }

            return promptToExecute;
        }

        public string Replay(string prompt, CancellationToken cancellationToken)
        {
            var promptToExecute = PromptContexts.BOB_ASSISTANT + PromptsManager.PutAgentSuffix(PromptsManager.PutUserPrefix(PromptsManager.CleanPromptInput(prompt)));
            StringBuilder responseBuilder = new StringBuilder();

            foreach (var response in LLamaModel.Executor.Infer(promptToExecute, LLamaModel.InferenceParams, cancellationToken))
            {
                responseBuilder.Append(response);
            }

            return responseBuilder.ToString();
        }
    }
}
