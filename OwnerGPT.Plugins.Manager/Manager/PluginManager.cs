using System.Reflection;

namespace OwnerGPT.Plugins.Manager.Manager
{
    public class PluginManager
    {
        public IList<string> PluginAssemblies;

        private static string PLUGIN_CONFIGURATION_PATH = "plugins.json";

        public PluginManager()
        {
            PluginAssemblies = new List<string>();
        }

        public string GetAssemblyPathByName(string assemblyName)
        {
            return Path.Combine(Directory.GetParent(Assembly.GetEntryAssembly()!.Location)!.FullName, assemblyName + ".dll");
        }

        public string[] GetPluginNames()
        {
            using(StreamReader reader = new StreamReader(PLUGIN_CONFIGURATION_PATH))
            {

            }
        }

        private string[] LoadPluginConfigurationFile()
        {

        }
    }
}
