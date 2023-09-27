using LLama;
using LLama.Common;
using OwnerGPT.LLM.Configuration;
using OwnerGPT.LLM.Interfaces;

namespace OwnerGPT.LLM.Models.LLama
{
    public class LLamaModel : ILLMModel
    {
        public InstructExecutor Executor { get; set; }
        public InferenceParams InferenceParams { get; set; }

        public LLamaModel()
        {
            string modelPath = Path.GetFullPath(ModelConfiguration.LLAMA_MODEL_PATH);

            if (!Path.Exists(modelPath))
            {
                throw new Exception("Model Not Found!");
            }

            var parameters = new ModelParams(modelPath)
            {
                ContextSize = ModelConfiguration.CONTEXT_SIZE,
                Seed = ModelConfiguration.SEED_COUNT,
                GpuLayerCount = ModelConfiguration.GPU_LAYER_COUNT,
            };

            Executor = new InstructExecutor(LLamaWeights.LoadFromFile(parameters).CreateContext(parameters));

            InferenceParams = new InferenceParams
            {
                Temperature = 0.75f,
                AntiPrompts = new List<string> { "Answer:" },
                MaxTokens = 512,
            };
        }
    }
}
