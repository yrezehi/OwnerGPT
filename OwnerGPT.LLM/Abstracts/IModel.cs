namespace OwnerGPT.LLM.Abstracts
{
    interface IModel
    {
        Task<string> Replay(string prompt);
    }
}
