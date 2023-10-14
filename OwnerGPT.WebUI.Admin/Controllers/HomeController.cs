using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class HomeController : BasePartialViewController
    {
        public HomeController() { }

        [HttpGet("[action]")]
        public IActionResult Index() =>
            View();

        [HttpGet("[action]")]
        public IActionResult KnowledgeBase() =>
            View();

        [HttpGet("[action]")]
        public IActionResult Tutorial() =>
            View();
    }
}