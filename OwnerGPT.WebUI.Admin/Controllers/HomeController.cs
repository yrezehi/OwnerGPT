using Microsoft.AspNetCore.Mvc;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;
using OwnerGPT.WebUI.Admin.Models;
using System.Diagnostics;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("[controller]")]
    public class HomeController : BasePartialViewController
    {
        public HomeController() { }

        [HttpGet("[action]")]
        public IActionResult Index() =>
            View();

        [HttpGet("[action]")]
        public IActionResult Chat() =>
            View();

        [HttpGet("[action]")]
        public IActionResult Search() =>
            View();

        [HttpGet("[action]")]
        public IActionResult KnowledgeBase() =>
            View();
    }
}