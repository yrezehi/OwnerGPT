using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Models.DTO.Interfaces;
using OwnerGPT.Services.Abstract.Interfaces;

namespace OwnerGPT.Controllers.Abstract
{
    public class RDBMSBaseController<IService, T, TDTO> : Controller where IService : IRDBMSCRUDService<T> where T : class where TDTO : class
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

        [HttpDelete("api")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            return Ok(await Service.Delete(id));
        }

        [HttpPut("api")]
        public virtual async Task<IActionResult> Update(T entityToUpdate, TDTO entityDTO)
        {
            return Ok(await Service.Update(entityToUpdate, (IDTO)entityDTO));
        }

        [HttpPost("api")]
        public virtual async Task<IActionResult> Insert([FromBody] TDTO entityDTO)
        {
            return Ok(await Service.Insert((IDTO)entityDTO));
        }

    }
}
