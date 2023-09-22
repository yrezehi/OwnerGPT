namespace OwnerGPT.LLM.PromptEnginnering
{
    public static class Prompts
    {

        public static string ANSWER_CONTEXT = "Answer the question based on the context below";
        public static string KEEP_ANSWER_SHORT = "Answer the question based on the context below:";
        public static string RESPOND_UNSURE_IF_YOUR = "Respond \"Unsure about answer\" if not sure about the answer";

        public static string GENERIC_ASSISTANT = @"Transcript of a dialog, where the User interacts with an Assistant named <NAME/>.
            <NAME/> is helpful, kind, honest, good at writing, and never fails to provide short answer to User's requests immediately and with precision.";

        private static string QUESTION_ANSWER = $@"
            {ANSWER_CONTEXT}. {KEEP_ANSWER_SHORT}. {RESPOND_UNSURE_IF_YOUR}.
            
            Context: <CONTEXT_DATA/>.

            Question: <INPUT_DATA/>.

            Answer:
        ";

        private static string CLASSIFIE_TEXT = @"
            Classify the input into <LIST_COMMA_DATA/>
            Input: <INPUT_DATA/>
            Sentiment:
        ";

        private static string SUMMARIZE = @"
            Write a concise and comprehensive summary of:
            <INPUT_DATA/>
            Summery:
        ";

    }
}
