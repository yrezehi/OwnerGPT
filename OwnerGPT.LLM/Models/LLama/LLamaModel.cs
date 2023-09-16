using LLama;
using LLama.Common;
using OwnerGPT.LLM.Configuration;
using OwnerGPT.LLM.Interfaces;
using OwnerGPT.LLM.PromptEnginnering;
using OwnerGPT.Models.Entities.Agents;
using System.Collections;
using System.Reflection;
using System.Text;

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
                ContextSize = ModelConfiguration.MODEL_CONTEXT_SIZE,
                Seed = ModelConfiguration.MODEL_SEED_COUNT,
                GpuLayerCount = ModelConfiguration.MODEL_GPU_LAYER_COUNT,
            };

            Executor = new InstructExecutor(LLamaWeights.LoadFromFile(parameters).CreateContext(parameters));

            InferenceParams = new InferenceParams
            {
                Temperature = 0.75f,
                AntiPrompts = new List<string> { "User:" },
                MaxTokens = 512,
            };
        }
    }
}
