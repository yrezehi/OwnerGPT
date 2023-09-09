namespace OwnerGPT.LLM.Interfaces
{
    interface ILLMModel
    {
        IEnumerable<string> Replay(string prompt);
    }
}
