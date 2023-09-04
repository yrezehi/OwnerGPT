using Microsoft.AspNetCore.Mvc;

namespace OwnerGPT.WebUI.Admin.Controllers.Abstract
{
    public partial class BasePartialViewController : Controller
    {
        [HttpGet("[action]")]
        public IActionResult GetPartialView(string viewName) => PartialView(viewName);
    }
}
