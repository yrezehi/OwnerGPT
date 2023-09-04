using OwnerGPT.Plugins.Parsers.WEB;
namespace Experimental.Plugins
{
    public static class WebPluginInstance
    {
        public async static Task Start()
        {
            WebPlugin plugin = new WebPlugin();

            var document = await plugin.Process("");

            Console.WriteLine(document);
        }
    }
}
