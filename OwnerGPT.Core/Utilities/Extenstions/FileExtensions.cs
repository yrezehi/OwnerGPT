using Microsoft.AspNetCore.Http;

namespace OwnerGPT.Core.Utilities.Extenstions
{
    public static class FileExtensions
    {
        public static byte[] GetBytes(this IFormFile formFile)
        {
            var binaryReader = new BinaryReader(formFile.OpenReadStream());
            return binaryReader.ReadBytes((int)formFile.Length);
        }

        public static string GetUniqueFileName(this IFormFile formFile) =>
            Guid.NewGuid().ToString() + "_" + formFile.Name;

        public static string GetExtension(this IFormFile formFile) =>
            Path.GetExtension(formFile.Name);

    }
}
