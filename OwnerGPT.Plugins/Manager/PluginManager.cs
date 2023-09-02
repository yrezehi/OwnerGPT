namespace OwnerGPT.Plugins.Manager
{
    public class PluginManager
    {
        public IList<string> PluginAssemblies;

        public PluginManager()
        {
            PluginAssemblies = new List<string>();
        }
    }
}
