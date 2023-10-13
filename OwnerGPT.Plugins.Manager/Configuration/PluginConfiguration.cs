using System.Text.Json;

namespace OwnerGPT.Plugins.Manager.Configuration
{
    public class PluginConfiguration
    {
        public List<string> Plugins { get; set; } = new();

        public static PluginConfiguration LoadFile(string configurationPath)
        {
            using (StreamReader reader = new StreamReader(configurationPath))
            {
                return JsonSerializer.Deserialize<PluginConfiguration>(reader.ReadToEnd())!;
            }
        }
    }
}
