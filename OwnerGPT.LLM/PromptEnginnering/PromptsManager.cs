namespace OwnerGPT.LLM.PromptEnginnering
{
    public static class PromptsManager
    {
        public static string PutAgentSuffix(string content) =>
            content + $"\r\nAssistant: ";
        

        public static string PutUserPrefix(string content) => 
            $"\r\nUser: {content}";

        public static string CleanPromptInput(string input) =>
            input.Trim();

    }
}
