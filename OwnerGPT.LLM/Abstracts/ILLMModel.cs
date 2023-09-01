namespace OwnerGPT.LLM.Abstracts
{
    interface ILLMModel
    {
        Task<string> Replay(string prompt);

        Task<string> ReplayWithInstruction(string instruction, string prompt);
    }
}
