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
            var agent = await AgentsService.NullableFindById(agentId);

            var promptToExecute = ( agent?.Instruction ?? Prompts.BOB_ASSISTANT ) + PromptsManager.PutAgentSuffix(PromptsManager.CleanPromptInput(prompt)/*PromptsManager.PutUserPrefix(PromptsManager.CleanPromptInput(prompt))*/);

            foreach (var response in LLamaModel.Executor.Infer(promptToExecute, LLamaModel.InferenceParams, cancellationToken))
            {
                yield return response;
            }
        }

        public string Replay(string prompt, CancellationToken cancellationToken)
        {
            var promptToExecute = Prompts.BOB_ASSISTANT + PromptsManager.PutAgentSuffix(PromptsManager.PutUserPrefix(PromptsManager.CleanPromptInput(prompt)));
            StringBuilder responseBuilder = new StringBuilder();

            foreach (var response in LLamaModel.Executor.Infer(promptToExecute, LLamaModel.InferenceParams, cancellationToken))
            {
                responseBuilder.Append(response);
            }

            return responseBuilder.ToString();
        }
    }
}
