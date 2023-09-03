using OwnerGPT.Plugins.Parsers.WEB;


WebPlugin plugin = new WebPlugin();

var document = await plugin.GetDocument("https://www.sfda.gov.sa/ar/overview");

Console.WriteLine("Class project finished execution!");