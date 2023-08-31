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
    }
}