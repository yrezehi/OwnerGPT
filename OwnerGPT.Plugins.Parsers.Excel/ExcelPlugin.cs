using Microsoft.AspNetCore.Http;
using OwnerGPT.Plugins.Manager.Interfaces;
using OwnerGPT.Plugins.Parsers.Excel.Loader;

namespace OwnerGPT.Plugins.Parsers.Excel
{
    public class ExcelPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public static async Task<string> Process(IFormFile file) =>
            CSVPromptFriendly(await LoadExcel.LoadAsCSV(file));

        private static string CSVPromptFriendly(IEnumerable<dynamic?> entites) =>
            string.Join("\n", entites.Where(entity => entity != null));
        
    }
}