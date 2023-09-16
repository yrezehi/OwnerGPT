using OwnerGPT.LLM.Models.LLama;
using OwnerGPT.LLM.PromptEnginnering;
using System.Text;

namespace OwnerGPT.Core.Services
{
    public class GPTService
    {
        LLamaModel LLamaModel { get; set; }

        public GPTService(LLamaModel llamaModel) { 
            LLamaModel = llamaModel;
        }

        public IEnumerable<string> StreamReplay(string prompt, CancellationToken cancellationToken)
        {
            var promptToExecute = Prompts.BOB_ASSISTANT + PromptsManager.PutAgentSuffix(PromptsManager.PutUserPrefix(PromptsManager.CleanPromptInput(prompt)));

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
