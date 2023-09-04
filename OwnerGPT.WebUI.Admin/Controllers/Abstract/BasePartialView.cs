using Microsoft.AspNetCore.Mvc;

namespace OwnerGPT.WebUI.Admin.Controllers.Abstract
{
    public class BasePartialView
    {
        public IActionResult PartialView(string viewName) => PartialView(viewName);
    }
}
