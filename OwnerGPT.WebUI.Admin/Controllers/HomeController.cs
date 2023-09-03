using Microsoft.AspNetCore.Mvc;
using OwnerGPT.WebUI.Admin.Models;
using System.Diagnostics;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        public HomeController() { }

        [HttpGet("[action]")]
        public IActionResult Index() =>
            View();

        [HttpGet("[action]")]
        public IActionResult Dashboard() =>
            View();

        [HttpGet("[action]")]
        public IActionResult Search() =>
            View();

        [HttpGet("[action]")]
        public IActionResult KnowledgeBase() =>
            View();
    }
}