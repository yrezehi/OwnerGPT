using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services;
using OwnerGPT.Models.Entities;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;
using OwnerGPT.WebUI.Admin.Models;
using System.Diagnostics;

namespace OwnerGPT.WebUI.Admin.Controllers
{
    [Route("")]
    public class AccountController : RDBMSBaseController<AccountService, Account>
    {
        public AccountController(AccountService Service) : base(Service) { }

        [HttpGet]
        public IActionResult Login() => 
            View();

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Manager() =>
            View(await Service.GetAll());
    }
}