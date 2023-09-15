using OwnerGPT.Models.Entities.Agents;

namespace OwnerGPT.LLM.PromptEnginnering
{
    public class PromptsManager
    {
        private static string DAN_11 = "PromptEnginner/Prompts/DAN_11.txt";

        public static string GetPrompt()
        {
            return ReadTextFile(DAN_11);
        }

        public static string PutAgentSuffix(string content)
        {
            return content + $"\nBob: ";
        }

        public static string PutUserPrefix(string content)
        {
            return $"User: {content}";
        }

        private static string ReadTextFile(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
