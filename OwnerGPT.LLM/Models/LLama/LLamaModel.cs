using LLama;
using LLama.Common;
using OwnerGPT.LLM.Abstracts;
using OwnerGPT.LLM.Configuration;
using System.Text;

namespace OwnerGPT.LLM.Models.LLama
{
    public class LLAMAModel : ILLMModel
    {
        private LLamaModel Model { get; set; }

        private StatelessExecutor Executor { get; set; }
        private InferenceParams InferenceParams { get; set; }
        private ModelParams ModelParameters { get; set; }

        public LLAMAModel()
        {
            Model = new LLamaModel(new ModelParams(
                ModelConfiguration.LLAMA_MODEL_PATH,
                contextSize: ModelConfiguration.MODEL_CONTEXT_SIZE,
                seed: ModelConfiguration.MODEL_SEED_COUNT,
                gpuLayerCount: ModelConfiguration.MODEL_GPU_LAYER_COUNT
            ));

            Executor = new StatelessExecutor(Model);

            InferenceParams = new InferenceParams
            {
                Temperature = 1.0f,
                AntiPrompts = new List<string> { "User:" },
                MaxTokens = 256
            };
        }

        public async Task<string> Replay(string prompt)
        {
            StringBuilder responseBuilder = new StringBuilder();

            await foreach (var response in Executor.InferAsync(prompt, InferenceParams))
            {
                Console.Write(response);
                responseBuilder.Append(response);
            }

            return responseBuilder.ToString();
        }

    }
}
