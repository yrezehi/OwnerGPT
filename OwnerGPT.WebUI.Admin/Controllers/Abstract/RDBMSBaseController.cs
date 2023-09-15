using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services.Abstract.Interfaces;

namespace OwnerGPT.WebUI.Admin.Controllers.Abstract
{
    [Route("[controller]")]
    public class RDBMSBaseController<IService, T> : Controller where IService : IRDBMSCRUDService<T> where T : class
    {
        public IService Service { get; set; }

        public RDBMSBaseController(IService service) => Service = service;

        [HttpGet("api/{id}")]
        public virtual async Task<IActionResult> Id(int id)
        {
            return Ok(await Service.FindById(id));
        }

        [HttpGet("api")]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok(await Service.GetAll());
        }

    }
}
