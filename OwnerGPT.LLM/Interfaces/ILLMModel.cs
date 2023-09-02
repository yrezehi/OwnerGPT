namespace OwnerGPT.LLM.Interfaces
{
    interface ILLMModel
    {
        Task<string> Replay(string prompt);

        Task<string> ReplayWithInstruction(string instruction, string prompt);
    }
}
