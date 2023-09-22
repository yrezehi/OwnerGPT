using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OwnerGPT.LLM.PromptEnginnering
{
    public static class PromptEngine
    {

        private static string DEFAULT_PROMPTS_PATH = "PromptEnginnering\\Prompts\\";
        private static string DEFAULT_INPUT_PLACEHOLDER = "<INPUT>";

        public static string Render(Prompts prompt, params string[] values)
        {
            string plainTemplate = PromptEngine.LoadPrompt(prompt);
            int substituteCounter = 0;

            return Regex.Replace(plainTemplate, Regex.Escape(DEFAULT_INPUT_PLACEHOLDER),(match) => values[substituteCounter <= values.Length ? values.Length : substituteCounter++]);
        }

        public static string LoadPrompt(Prompts prompt)
        {
            string filePath = BuildFilePath(prompt);

            if (!Path.Exists(filePath))
                throw new Exception("File does not exists!");

            return PromptEngine.ReadTextFile(filePath);
        }

        public static string BuildFilePath(Prompts prompt)
        {
            return Path.Combine(DEFAULT_PROMPTS_PATH, Enum.GetName(typeof(Prompts), prompt)! + ".txt");
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
