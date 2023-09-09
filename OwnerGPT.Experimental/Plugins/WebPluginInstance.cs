using OwnerGPT.Plugins.Parsers.WEB;
namespace OwnerGPT.Experimental.Plugins
{
    public static class WebPluginInstance
    {
        public async static Task Start(string url)
        {
            WebPlugin plugin = new WebPlugin();

            var document = await plugin.Process(url);

            Console.WriteLine(document);
        }
    }
}
