using LLama;
using LLama.Common;
using OwnerGPT.LLM.Configuration;
using OwnerGPT.LLM.Interfaces;
using OwnerGPT.LLM.PromptEnginnering;
using System.Collections;
using System.Reflection;
using System.Text;

namespace OwnerGPT.LLM.Models.LLama
{
    public class LLAMAModel : ILLMModel
    {
        private InstructExecutor Executor { get; set; }
        private InferenceParams InferenceParams { get; set; }

        public LLAMAModel()
        {
            string modelPath = Path.GetFullPath(ModelConfiguration.LLAMA_MODEL_PATH);

            if (!Path.Exists(modelPath))
            {
                throw new Exception("Model Not Found!");
            }

            var parameters = new ModelParams(modelPath)
            {
                ContextSize = ModelConfiguration.MODEL_CONTEXT_SIZE,
            };

            Executor = new InstructExecutor(LLamaWeights.LoadFromFile(parameters).CreateContext(parameters));

            InferenceParams = new InferenceParams
            {
                Temperature = 1.0f,
                MaxTokens = 2560,
            };

            Executor.Infer(Prompts.BOB_ASSISTANT, InferenceParams);
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
