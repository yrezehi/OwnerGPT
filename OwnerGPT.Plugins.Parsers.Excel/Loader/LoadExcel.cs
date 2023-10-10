using Microsoft.AspNetCore.Http;
using MiniExcelLibs;

namespace OwnerGPT.Plugins.Parsers.Excel.Loader
{
    public static class LoadExcel
    {
        public async static Task<List<dynamic?>> Load(IFormFile file) =>
            MiniExcel.Query(await file.AsStream()).ToList();

        public async static Task<IEnumerable<dynamic?>> LoadAsCSV(IFormFile file)
        {
            var csvStream = new MemoryStream();

            MiniExcel.ConvertXlsxToCsv(await file.AsStream(), csvStream);
            csvStream.Position = 0;

            return MiniExcel.Query(csvStream, useHeaderRow: true, excelType: ExcelType.CSV);
        }

        private async static Task<Stream> AsStream(this IFormFile file)
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
