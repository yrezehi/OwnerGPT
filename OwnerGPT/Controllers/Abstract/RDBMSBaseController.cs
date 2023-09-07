﻿using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Services.Abstract.Interfaces;

namespace OwnerGPT.Controllers.Abstract
{
    [Route("api/[controller]")]
    public class RDBMSBaseController<IService, T> : Controller where IService : IRDBMSCRUDService<T> where T : class
    {
        public IService Service { get; set; }

        public RDBMSBaseController(IService service) => Service = service;

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Id(int id)
        {
            return Ok(await Service.FindById(id));
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok(await Service.GetAll());
        }

    }
}
