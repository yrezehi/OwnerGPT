namespace OwnerGPT.LLM.Interfaces
{
    interface ILLMModel
    {
        IEnumerable<string> StreamReplay(string prompt);
    }
}
