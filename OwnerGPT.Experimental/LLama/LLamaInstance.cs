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
            var gpuLayers = 5;
            var contextLength = 1337;

            var modelOptions = new LlamaCppModelOptions
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

            using var model = new LlamaCppModel();
            model.Load(modelPath, modelOptions);

            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) => cancellationTokenSource.Cancel(!(e.Cancel = true));

            var generateOptions = new LlamaCppGenerateOptions { Temperature = 0.0f, Mirostat = Mirostat.Disabled, ThreadCount = 4 };
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
            const string B_INST = "[INST]";
            const string E_INST = "[/INST]";
            const string B_SYS = "<<SYS>>\n";
            const string E_SYS = "\n<</SYS>>\n\n";
            const string SYS_PROMPT = "You are a helpful assistant.";
            // ------------------------------------------------------------------------------------------------------------------------------

            var first = true;

            while (true)
            {
                await Console.Out.WriteLineAsync("\nInput:");

                var userPrompt = await Console.In.ReadLineAsync() ?? String.Empty;
                if (String.IsNullOrWhiteSpace(userPrompt))
                    break;

                var prompt = $"{B_INST} {(first ? $"{B_SYS}{SYS_PROMPT}{E_SYS}" : "")}{userPrompt} {E_INST} ";

                var match = Regex.Match(userPrompt, @"^\/(?<Command>\w+)\s?""?(?<Arg>.*?)""?$");
                if (match.Success)
                {
                    var command = match.Groups["Command"].Value.ToLower();
                    var arg = match.Groups["Arg"].Value;

                    if (command == "load")
                    {
                        var path = Path.GetFullPath(arg);
                        await Console.Out.WriteAsync($"Loading prompt from \"{path}\"...");
                        if (!File.Exists(path))
                        {
                            await Console.Out.WriteLineAsync($" [File not found].");
                            continue;
                        }
                        prompt = File.ReadAllText(arg);
                        var tokenCount = model.Tokenize(prompt, true).Count;
                        await Console.Out.WriteLineAsync($" [{tokenCount} token(s)].");
                        if (tokenCount == 0 || tokenCount >= contextLength - 4)
                        {
                            await Console.Out.WriteLineAsync($"Context limit reached ({contextLength}).");
                            continue;
                        }
                        session.Reset();
                        //model.ResetState();
                    }
                    else if (command == "reset")
                    {
                        session.Reset();
                        model.ResetState();
                        await Console.Out.WriteLineAsync($"Context reset.");
                        continue;
                    }
                    else if (command == "dump")
                    {
                        var separator = new String('=', Console.WindowWidth);
                        await Console.Out.WriteLineAsync(separator);
                        await Console.Out.WriteLineAsync(session.GetContextAsText());
                        await Console.Out.WriteLineAsync(separator);
                        continue;
                    }
                }

                await Console.Out.WriteLineAsync("\nOutput:");

                await foreach (var tokenString in session.GenerateTokenStringAsync(prompt, generateOptions, cancellationTokenSource.Token))
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
