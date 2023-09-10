namespace OwnerGPT.LLM.PromptEnginnering
{
    public class PromptEnginner
    {
        private static string DAN_11 = "PromptEnginner/Prompts/DAN_11.txt";

        public static string GetPrompt()
        {
            return ReadTextFile(DAN_11);
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
