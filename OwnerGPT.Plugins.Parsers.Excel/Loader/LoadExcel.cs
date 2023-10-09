using Microsoft.AspNetCore.Http;
using MiniExcelLibs;

namespace OwnerGPT.Plugins.Parsers.Excel.Loader
{
    public static class LoadExcel
    {
        public async static Task<List<dynamic?>> Load(IFormFile file) =>
            MiniExcel.Query(await file.AsStream()).ToList();

        private async static Task<Stream> AsStream(this IFormFile file)
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
