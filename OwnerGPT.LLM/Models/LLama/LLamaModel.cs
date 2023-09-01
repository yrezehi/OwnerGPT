using LLama;
using LLama.Common;
using System.Text;

namespace OwnerGPT.LLM.Models.LLama
{
    public class LLAMAModel
    {
        private LLamaModel Model { get; set; }

        private StatelessExecutor Executor { get; set; }
        private InferenceParams InferenceParams { get; set; }

        public LLAMAModel()
        {
            Model = new LLamaModel(new ModelParams("TODO:ME", contextSize: 1024, seed: 1337, gpuLayerCount: 5));
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
