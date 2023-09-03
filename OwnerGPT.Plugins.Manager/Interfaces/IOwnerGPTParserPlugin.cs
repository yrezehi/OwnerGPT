namespace OwnerGPT.Plugins.Manager.Interfaces
{
    public interface IOwnerGPTParserPlugin<T, E> {
        Task<E> Process(T content);
    }
}
