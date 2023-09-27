using OwnerGPT.LLM.Native;
using OwnerGPT.LLM.PromptEnginnering;
using System.Text.RegularExpressions;

namespace OwnerGPT.Experimental.LLMManager
{
    public class LLamaInstance
    {
        public async static Task Start()
        {
            var modelPath = "C:\\llm_models\\llama-2-7b-guanaco-qlora.Q5_K_S.gguf";
            var gpuLayers = 42;
            var contextLength = 16384;

            var modelOptions = new NativeLLamaOptions
            {
                Seed = 0,
                ContextSize = contextLength,
                GpuLayers = gpuLayers,
                //RopeFrequencyBase = 10000.0f,
                //RopeFrequencyScale = 0.5f,
                //LowVRAM = true,
                //UseMemoryLocking = false,
                //UseMemoryMapping = false,
            };

            using var model = new NativeLLamaModel();
            model.Load(modelOptions);

            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) => cancellationTokenSource.Cancel(!(e.Cancel = true));

            var generateOptions = new NativeLlamaGenerateOptions { Temperature = 0.0f, Mirostat = Mirostat.Disabled, ThreadCount = 4 };

            var session = model.CreateSession();

            await Console.Out.WriteLineAsync(
                """

                Entering interactive mode:
                    * Press <Ctrl+C> to cancel running predictions
                    * Press <Enter> on an empty input prompt to quit
                """
            );

            // ------------------------------------------------------------------------------------------------------------------------------
            // Llama-2
            // ------------------------------------------------------------------------------------------------------------------------------
            // <s>[INST] <<SYS>>
            // {{ system_prompt }}
            // <</SYS>>
            //
            // {{ user_msg_1 }} [/INST] {{ model_answer_1 }} </s>\
            // <s>[INST] {{ user_msg_2 }} [/INST] {{ model_answer_2 }} </s>\
            // <s>[INST] {{ user_msg_3 }} [/INST]
            //
            // https://github.com/facebookresearch/llama/blob/6c7fe276574e78057f917549435a2554000a876d/llama/generation.py#L250
            //
            // self.tokenizer.encode(
            //     f"{B_INST} {(prompt['content']).strip()} {E_INST} {(answer['content']).strip()} ",
            //     bos=True,
            //     eos=True,
            // )
            // ------------------------------------------------------------------------------------------------------------------------------
            const string SYS_PROMPT = "You are a helpful assistant.";
            // ------------------------------------------------------------------------------------------------------------------------------

            var first = true;

            while (true)
            {
                await Console.Out.WriteLineAsync("\nInput:");

                var userPrompt = await Console.In.ReadLineAsync() ?? String.Empty;
                if (String.IsNullOrWhiteSpace(userPrompt))
                    break;

                var prompt = $"{SYS_PROMPT}";

                await Console.Out.WriteLineAsync("\nOutput:");

                await foreach (var tokenString in session.GenerateStatelessTokenStringAsync(prompt, generateOptions, cancellationTokenSource.Token))
                {
                    await Console.Out.WriteAsync(tokenString);
                }

                if (cancellationTokenSource.IsCancellationRequested)
                {
                    await Console.Out.WriteAsync(" [Cancelled]");
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = new();
                }

                await Console.Out.WriteLineAsync();
                first = false;
            }
        }
    }
}
