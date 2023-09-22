namespace OwnerGPT.LLM.PromptEnginnering
{
    public static class PromptsManager
    {
        public static string PutAgentSuffix(string content)
        {
            return content + $"\r\nAssistant: ";
        }

        public static string PutUserPrefix(string content)
        {
            return $"\r\nUser: {content}";
        }

        public static string CleanPromptInput(string input)
        {
            return input.Trim();
        }

        private static string ReadTextFile(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static string Render()
        {

        }
    }
}
