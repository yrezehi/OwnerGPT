using OwnerGPT.Plugins.Manager.Configuration;
using System.Reflection;

namespace OwnerGPT.Plugins.Manager.Manager
{
    public class PluginManager
    {
        public IList<string> PluginAssemblies;
        
        public PluginConfiguration PluginConfiguration;

        private static string PLUGIN_CONFIGURATION_PATH = "plugins.json";

        public PluginManager()
        {
            PluginAssemblies = new List<string>();
            PluginConfiguration = PluginConfiguration.LoadFile(PLUGIN_CONFIGURATION_PATH);
        }

        public string GetAssemblyPathByName(string assemblyName)
        {
            return Path.Combine(Directory.GetParent(Assembly.GetEntryAssembly()!.Location)!.FullName, assemblyName + ".dll");
        }

    }
}
