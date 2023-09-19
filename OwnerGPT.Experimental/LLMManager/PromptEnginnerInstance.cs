using OwnerGPT.LLM.PromptEnginnering;

namespace OwnerGPT.Experimental.LLMManager
{
    public class PromptEnginnerInstance
    {
        public async static Task Start()
        {
            var prompt = PromptsManager.GetPrompt();

            Console.WriteLine(prompt);
        }
    }
}
