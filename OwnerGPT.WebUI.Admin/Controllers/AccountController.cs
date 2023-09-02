﻿using Microsoft.AspNetCore.Mvc;
using OwnerGPT.WebUI.Admin.Models;
using System.Diagnostics;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login() => 
            View();

        [HttpGet("[controller]/[action]")]
        public IActionResult Manager() =>
            View();
    }
}