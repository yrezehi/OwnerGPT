using LLama.Common;
using LLama;
using System.Text;

namespace OwnerGPT.LLM.Services
{
    public class StatelessGPTService
    {
        private readonly StatelessExecutor StatelessExecutor;
        private InferenceParams InferenceParameters;

        public StatelessGPTService()
        {
            StatelessExecutor = new StatelessExecutor((new LLamaModel(new ModelParams("C:\\llm_model\\wizardLM.bin", contextSize: 1024, seed: 1337, gpuLayerCount: 5))));
            InferenceParameters = new InferenceParams
            {
                Temperature = 1.0f,
                AntiPrompts = new List<string> { "Question:", "#", "Question: ", ".\n" },
                MaxTokens = 256
            };
        }

        public async Task<string> Replay(string question)
        {
            StringBuilder replayBuilder = new StringBuilder();

            await foreach(var replaySegment in StatelessExecutor.InferAsync($"Question: {question} Answer: ", inferenceParams: InferenceParameters))
            {
                replayBuilder.AppendLine(replaySegment);
            }

            return replayBuilder.ToString();
        }
    }

}
