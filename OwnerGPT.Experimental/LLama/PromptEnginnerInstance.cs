using OwnerGPT.LLM.PromptEnginnering;

namespace OwnerGPT.Experimental.LLMManager
{
    public class PromptEnginnerInstance
    {
        public async static Task Start()
        {
            var prompt = TemplateEngine.LoadPrompt(Prompts.GENERIC_ASSISTANT);

            Console.WriteLine(prompt);
        }
    }
}
