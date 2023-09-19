using Microsoft.AspNetCore.Mvc;
using OwnerGPT.Core.Services;
using OwnerGPT.Models.Entities;
using OwnerGPT.WebUI.Admin.Controllers.Abstract;

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
            View(await Service.RDBMSServiceBase.GetAll());
    }
}