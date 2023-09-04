using OwnerGPT.LLM.PromptEnginner;
using System.Reflection.Metadata;

namespace Experimental.LLMManager
{
    public class PromptEnginnerInstance
    {
        public async static Task Start()
        {
            var prompt = PromptEnginner.GetPrompt();

            Console.WriteLine(prompt);
        }
    }
}
