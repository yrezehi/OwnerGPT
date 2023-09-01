using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Models;
using OwnerGPT.Services;
using System.Reflection.Metadata;

namespace OwnerGPT.Controllers
{
    [Route("api/[controller]")]
    public class ContextController : Controller
    {
        private readonly VectorEmbeddingService Service;

        public ContextController(VectorEmbeddingService service) =>
            Service = service;

        [HttpPost]
        public async Task<IActionResult> Context(string context) =>
            Ok(await Service.Insert(context));

        [HttpPost("[action]")]
        public async Task<IActionResult> NearestContext(string query) =>
            Ok(await Service.NearestNeighbor(query));

        [HttpGet("[action]")]
        public async Task<IActionResult> Contexts() =>
            Ok(await Service.All());
        
    }
}