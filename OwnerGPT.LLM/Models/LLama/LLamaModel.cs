using LLama;
using LLama.Common;
using OwnerGPT.LLM.Configuration;
using OwnerGPT.LLM.Interfaces;
using System.Collections;
using System.Reflection;
using System.Text;

namespace OwnerGPT.LLM.Models.LLama
{
    public class LLAMAModel : ILLMModel
    {
        private InteractiveExecutor Executor { get; set; }
        private InferenceParams InferenceParams { get; set; }

        public LLAMAModel()
        {
            var parameters = new ModelParams(ModelConfiguration.LLAMA_MODEL_PATH)
            {
                ContextSize = ModelConfiguration.MODEL_CONTEXT_SIZE,
                Seed = ModelConfiguration.MODEL_SEED_COUNT,
                GpuLayerCount = ModelConfiguration.MODEL_GPU_LAYER_COUNT
            };

            Executor = new InteractiveExecutor(LLamaWeights.LoadFromFile(parameters).CreateContext(parameters));

            InferenceParams = new InferenceParams
            {
                Temperature = 1.0f,
                AntiPrompts = new List<string> { "User:" },
                MaxTokens = 256
            };
        }

        public IEnumerable<string> StreamReplay(string prompt)
        {
            foreach (var response in Executor.Infer(prompt, InferenceParams))
            {
                yield return response;
            }
        }

        public string Replay(string prompt)
        {
            StringBuilder responseBuilder = new StringBuilder();

            foreach (var response in Executor.Infer(prompt, InferenceParams))
            {
                responseBuilder.Append(response);
            }

            return responseBuilder.ToString();
        }

    }
}
