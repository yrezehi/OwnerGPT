using OwnerGPT.LLM.PromptEnginnering;

namespace OwnerGPT.Experimental.LLMManager
{
    public class PromptEnginnerInstance
    {
        public async static Task Start()
        {
            var prompt = PromptEngine.LoadPrompt(Prompts.GENERIC_ASSISTANT);

            Console.WriteLine(prompt);
        }
    }
}
