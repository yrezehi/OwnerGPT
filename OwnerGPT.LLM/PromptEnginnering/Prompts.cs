using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.LLM.PromptEnginnering
{
    public static class Prompts
    {
        private static string SUMMARIZE = @"
            Write a concise and comprehensive summary of:
            <INPUT_DATA>
            Summery:
        ";

        private static string QUESTION_ANSWER = @"
            Answer the question based on the context below. Keep the answer short and concise. Respond ""Unsure about answer"" if not sure about the answer.
            Context: <CONTEXT_DATA>
            Question: <INPUT_DATA>
            Answer:
        ";

        private static string CLASSIFIE_TEXT = @"
            Classify the input into <LIST_COMMA_DATA>
            Input: <INPUT_DATA>
            Sentiment:
        ";
    }
}
