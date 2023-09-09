using Microsoft.AspNetCore.Mvc;
using OwnerGPT.LLM.Models.LLama;
using System.IO;
using System;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("api/[controller]")]
    public class GPTController : Controller
    {
        private readonly LLAMAModel StatelessGPT;

        public GPTController(LLAMAModel statelessGPT)
        {
            StatelessGPT = statelessGPT;
        }

        [HttpPost("[action]")]
        public async void Replay(string message)
        {
            Response.StatusCode = 200;
            Response.ContentType = "text/plain";

            var streamWriter = new StreamWriter(Response.Body);
            
            foreach(var replay in StatelessGPT.Replay(message))
            {
                await streamWriter.WriteLineAsync(replay);
                await streamWriter.FlushAsync();
            }
        }
    }
}
