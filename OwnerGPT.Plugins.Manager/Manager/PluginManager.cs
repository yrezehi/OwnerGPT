using System.Reflection;

namespace OwnerGPT.Plugins.Manager.Manager
{
    public class PluginManager
    {
        public IList<string> PluginAssemblies;

        public PluginManager()
        {
            PluginAssemblies = new List<string>();
        }

        public string GetAssemblyPathByName(string assemblyName)
        {
            return Path.Combine(Directory.GetParent(Assembly.GetEntryAssembly()!.Location)!.FullName, assemblyName + ".dll");
        }
    }
}
