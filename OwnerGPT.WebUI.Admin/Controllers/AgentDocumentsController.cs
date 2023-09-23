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
            string path = Path.GetFullPath("C:\\ownergpt_files");
            string fileName = Path.GetFileName(file.FileName);

            await using (var streamWriter = new StreamWriter(Response.Body))
            {
                using (FileStream fileStream = System.IO.File.Create(Path.Combine(path, fileName)))
                using (Stream stream = file.OpenReadStream())
                {

                    byte[] streamBuffer = new byte[16 * 1024];
                    int bytesToProcess;
                    long totalReadBytes = 0;
                    int progress = 0;

                    while ((bytesToProcess = stream.Read(streamBuffer, 0, streamBuffer.Length)) > 0)
                    {
                        fileStream.Write(streamBuffer, 0, bytesToProcess);
                        totalReadBytes += bytesToProcess;
                        progress = (int)((float)totalReadBytes / (float)file.Length * 100.0);

                        await streamWriter.WriteLineAsync(progress.ToString());
                        await streamWriter.FlushAsync();
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
