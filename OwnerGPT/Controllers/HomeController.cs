﻿using Microsoft.AspNetCore.Mvc;

namespace OwnerGPT.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            return View();
        }
    }
}