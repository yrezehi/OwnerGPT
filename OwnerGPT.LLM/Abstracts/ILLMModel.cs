namespace OwnerGPT.LLM.Abstracts
{
    interface ILLMModel
    {
        Task<string> Replay(string prompt);
    }
}
