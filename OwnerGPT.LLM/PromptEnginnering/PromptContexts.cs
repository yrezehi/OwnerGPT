namespace OwnerGPT.LLM.PromptEnginnering
{
    public static class PromptContexts
    {
        public static Dictionary<string, string> PROMPTS = new()
        {
            {
                "GENERIC_ASSISTANT",
                @"Transcript of a dialog, where the User interacts with an Assistant named <INPUT>.<INPUT> is helpful, kind, honest, good at writing, and never fails to provide short answer to User's requests immediately and with precision."
            },
            {
                "QUESTION_ANSWER",
                $@"
                    Answer the question based on the context below. Answer the question based on the context below:. Respond ""Unsure about answer"" if not sure about the answer.
                    Context: <INPUT>.
                    Question: <INPUT>.
                    Answer:
                "
            },
            {
                "SUMMARIZE",
                @"
                    Write a concise and comprehensive summary of:
                    <INPUT>
                    Summery:
                "
            }
        };
    }
}
