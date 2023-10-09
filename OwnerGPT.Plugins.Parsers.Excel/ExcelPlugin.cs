using Microsoft.AspNetCore.Http;
using OwnerGPT.Plugins.Manager.Interfaces;
using OwnerGPT.Plugins.Parsers.Excel.Loader;

namespace OwnerGPT.Plugins.Parsers.Excel
{
    public class ExcelPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public static Task<List<dynamic?>> Process(IFormFile file) =>
            LoadExcel.Load(file);
    }
}