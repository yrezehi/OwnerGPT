using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services;
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
            Response.StatusCode = 200;
            Response.ContentType = "text/html";

            var streamWriter = new StreamWriter(Response.Body);

            await foreach (int progress in Service.StreamCreate(file))
            {
                await streamWriter.WriteLineAsync(progress.ToString());

                await streamWriter.FlushAsync();

                Thread.Sleep(100);
            }

        }

        // TODO: map agent
        [HttpGet("[action]/{documentId}")]
        public async Task<IActionResult> FetchPreview(int documentId) =>
            Ok(await Service.FetchPreview(documentId));

        [HttpGet("api")]
        public virtual async Task<IActionResult> GetAll() =>
            Ok(await Service.RDBMSServiceBase.GetAll());
      
    }
}
