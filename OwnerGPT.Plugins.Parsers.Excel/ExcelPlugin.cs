using Microsoft.AspNetCore.Http;
using OwnerGPT.Plugins.Manager.Interfaces;
using OwnerGPT.Plugins.Parsers.Excel.Loader;

namespace OwnerGPT.Plugins.Parsers.Excel
{
    public class ExcelPlugin : IOwnerGPTParserPlugin<string, string>
    {
        public static async Task<string> Process(IFormFile file) =>
            Serilize(await LoadExcel.Load(file));

        public static string Serilize(List<dynamic?> rows)
        {
            Dictionary<string, List<string>> serilizedRows = new();

            foreach (var row in rows.Cast<IDictionary<string, object>>())
            {
                foreach (var cell in row)
                {
                    if (!serilizedRows.ContainsKey(cell.Key))
                        serilizedRows.TryAdd(cell.Key, new List<string>());

                    if (cell.Value != null)
                    {
                        serilizedRows[cell.Key].Add(cell.Value.ToString()!);
                    }
                }
            }

            return serilizedRows.ToString();
        }
    }
}