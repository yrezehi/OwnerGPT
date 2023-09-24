using Microsoft.AspNetCore.Http;
using System.IO;

namespace OwnerGPT.Plugins.Manager.Documents.Configuration
{
    public static class FileExtensions
    {
        public static byte[] GetBytes(this IFormFile formFile)
        {
            var binaryReader = new BinaryReader(formFile.OpenReadStream());
            return binaryReader.ReadBytes((int)formFile.Length);
        }
    }
}
