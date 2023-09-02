namespace OwnerGPT.Plugins.Manager.Interfaces
{
    public interface IOwnerGPTParserPlugin {
        // the process of acquiring the document, whether getting it via internet, local file system or other objects
        Task<E> Get<T, E>(T content);

        // the process of converting the content to text representation
        Task<string> ToText<T>(T data);

        // the process of removing nosies of a 
        Task<string> Cleansing(string content);
    }
}
