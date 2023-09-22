using Microsoft.AspNetCore.Http;

namespace OwnerGPT.Core.Utilities.Extenstions
{
    public static class FileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static string GetUniqueFileName(this IFormFile formFile) =>
            Guid.NewGuid().ToString() + "_" + formFile.Name;

        public static string GetExtension(this IFormFile formFile) =>
            Path.GetExtension(formFile.Name);
    }
}
