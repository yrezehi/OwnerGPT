using LLama;
using LLama.Common;
using OwnerGPT.LLM.Configuration;
using OwnerGPT.LLM.Interfaces;

namespace OwnerGPT.LLM.Models.LLama
{
    public class LLamaModel : ILLMModel
    {
        public StatelessExecutor Executor { get; set; }
        public InferenceParams InferenceParams { get; set; }

        public LLamaModel()
        {
            string modelPath = Path.GetFullPath(ModelConfiguration.LLAMA_MODEL_PATH);

            if (!File.Exists(modelPath))
            {
                throw new ArgumentException("Model Not Found!");
            }

            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 1024 * 4,
                Seed = 0,
                GpuLayerCount = 0,
            };

            Executor = new StatelessExecutor(LLamaWeights.LoadFromFile(parameters).CreateContext(parameters));

            InferenceParams = new InferenceParams
            {
                Temperature = ModelConfiguration.TEMPERATURE,
                AntiPrompts = new List<string> { "Answer:" },
                MaxTokens = ModelConfiguration.MAX_TOKENS,
            };
        }
    }
}
