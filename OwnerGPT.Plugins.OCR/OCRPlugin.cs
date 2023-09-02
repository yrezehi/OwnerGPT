using OwnerGPT.Plugins.Manager.Interfaces;

namespace OwnerGPT.Plugins.OCR
{
    public class OCRPlugin : IOwnerGPTPlugin
    {
        public Task<string> Cleansing(string content)
        {
            throw new NotImplementedException();
        }

        public Task<E> Get<T, E>(T content)
        {
            throw new NotImplementedException();
        }

        public Task<string> ToText<T>(T data)
        {
            throw new NotImplementedException();
        }
    }
}