using Microsoft.AspNetCore.Http;
using OwnerGPT.Plugins.Manager.Interfaces;
using OwnerGPT.Plugins.Parsers.Excel.Loader;

namespace OwnerGPT.Plugins.Parsers.Excel
{
    public class ExcelPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public static async Task<string> Process(IFormFile file) =>
            CSVPromptFriendly(await LoadExcel.LoadAsCSV(file));

        private static string CSVPromptFriendly(IEnumerable<dynamic?> entites)
        {
            var enteries = new List<string>();

            foreach (var entity in entites.Cast<IDictionary<string, object>>())
            {
                enteries.Add(string.Join(", ", entity.Select(entity => $"{entity.Key}: {entity.Value}")));
            }

            return string.Join("\n", enteries);
        }

    }
}