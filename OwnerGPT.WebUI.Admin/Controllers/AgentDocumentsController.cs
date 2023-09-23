using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services;
using OwnerGPT.Models.Abstracts.Bindings;
using OwnerGPT.Models.Abstracts.DTO;
using OwnerGPT.Models.Agents;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    public class AgentDocumentsController : RDBMSBaseController<AgentDocumentsService, AgentDocument>
    {
        public AgentDocumentsController(AgentDocumentsService Service) : base(Service) { }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async void Upload(IFormFile file)
        {
            await using (var streamWriter = new StreamWriter(Response.Body))
            {
                await foreach (int progress in Service.StreamCreate(file))
                {
                    await streamWriter.WriteLineAsync(progress.ToString());
                    
                    await streamWriter.FlushAsync();

                    Thread.Sleep(100);
                }
            }
        }
    }
}
