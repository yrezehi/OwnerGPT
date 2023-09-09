using LLama.Common;
using LLama;
using System.Text;
using static LLama.LLamaTransforms;

namespace OwnerGPT.LLM.Services
{
    public class StatelessGPTService
    {
        private readonly StatelessExecutor StatelessExecutor;

        public StatelessGPTService()
        {
            // TODO: replace with a stateless executor
            StatelessExecutor = new StatelessExecutor((new LLamaModel(new ModelParams("C:\\llm_model\\wizardLM.bin", contextSize: 1024, seed: 1337, gpuLayerCount: 5))));
        }

        public async Task<string> Replay(ChatHistory history)
        {
            var result = _session.ChatAsync(history, new InferenceParams()
            {
                AntiPrompts = new string[] { "User:" },
            });

            var sb = new StringBuilder();
            await foreach (var r in result)
            {
                Console.Write(r);
                sb.Append(r);
            }

            return sb.ToString();

        }
    }
    public class HistoryTransform : DefaultHistoryTransform
    {
        public override string HistoryToText(ChatHistory history)
        {
            return base.HistoryToText(history) + "\n Assistant:";
        }

    }
}
