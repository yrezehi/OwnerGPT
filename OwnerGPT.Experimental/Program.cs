using OwnerGPT.Experimental.Plugins;

//await PromptEnginnerInstance.Start();
await WebPluginInstance.Start("https://www.google.com/search?q=github&oq=github&aqs=chrome.0.0i271j46i67i131i199i433i465i650j69i59j69i60l5.3942j0j7&sourceid=chrome&ie=UTF-8");

Console.WriteLine("Class project finished execution!");