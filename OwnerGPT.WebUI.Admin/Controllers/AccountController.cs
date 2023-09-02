using Microsoft.AspNetCore.Mvc;
using OwnerGPT.WebUI.Admin.Models;
using System.Diagnostics;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpGet("[Action]")]
        public IActionResult Login()
        {
            return View();
        }

    }
}