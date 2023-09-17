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

        public static string BOB_ASSISTANT = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to provide short answer to User's requests immediately and with precision.\r\nUser: Hello, Bob\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.";

        public static string HELPFUL_ASSISTANT = "You are a helpful, respectful and honest assistant. Always answer as helpfully as possible, while being safe.  Your answers should not include any harmful, unethical, racist, sexist, toxic, dangerous, or illegal content. Please ensure that your responses are socially unbiased and positive in nature.\r\n\r\nIf a question does not make any sense, or is not factually coherent, explain why instead of answering something not correct. If you don't know the answer to a question, please don't share false information.\r\n";
    }
}
