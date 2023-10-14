﻿using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Extensions;
using OwnerGPT.Core.Services;
using OwnerGPT.Models;
using OwnerGPT.Models.Abstracts.DTO;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("")]
    public class AccountController : RDBMSBaseController<AccountService, Account>
    {
        public AccountController(AccountService Service) : base(Service) { }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Manager(int? page) =>
            View(await Service.RDBMSServiceBase.GetAll(page));

        [HttpGet("[controller]/[action]")]
        public IActionResult RequestAccess() => View();

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> SignOut()
        {
            await Service.SignOut();

            return RedirectToAction("Login");
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> SignIn([FromForm] CredentialsDTO credintalsDTO) =>
            await Service.SignIn(credintalsDTO)
                .Then(_ => RedirectToAction("Index", "Home"));
    }
}