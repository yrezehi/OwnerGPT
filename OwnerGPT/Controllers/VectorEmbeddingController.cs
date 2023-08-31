using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Services;

namespace OwnerGPT.Controllers
{
    [Route("api/[controller]")]
    public class VectorEmbeddingController : Controller
    {
        private readonly VectorEmbeddingService Service;

        public VectorEmbeddingController(VectorEmbeddingService service) {
            Service = service;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Id(int id)
        {
            Service.Insert(new Pgvector.Vector(new float[] { 1, 1, 1}), "context");
            var x = await Service.NearestNeighbor(new Pgvector.Vector(new float[] { 1, 1, 1 }));
            return Ok();
        }
    }
}