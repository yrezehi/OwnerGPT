using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services.Compositions;

namespace OwnerGPT.WebUI.Admin.Controllers.Abstract
{
    [Route("[controller]")]
    public class RDBMSBaseController<IService, T> : Controller where IService : CompositionBaseService<T> where T : class
    {
        public IService Service { get; set; }

        public RDBMSBaseController(IService service) => Service = service;

        [HttpGet("api/{id}")]
        public virtual async Task<IActionResult> Id(int id)
        {
            return Ok(await Service.RDBMSServiceBase.FindById(id));
        }

        [HttpGet("api")]
        public virtual async Task<IActionResult> GetAll(int? page)
        {
            return Ok(await Service.RDBMSServiceBase.GetAll(page));
        }

        [HttpGet("api/[action]")]
        public virtual async Task<IActionResult> Search(string property, string value, int? page)
        {
            return Ok(await Service.RDBMSServiceBase.SearchByProperty<string>(property, value, page));
        }

    }
}
