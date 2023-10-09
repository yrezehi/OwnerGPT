using Microsoft.AspNetCore.Http;
using OwnerGPT.Plugins.Manager.Interfaces;

namespace OwnerGPT.Plugins.Parsers.Excel
{
    public class ExcelPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public Task<dynamic?> Process(IFormFile file) =>
            file.OpenReadStream();
    }
}