using Microsoft.AspNetCore.Mvc;

namespace Projekt_WEB.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("{code:int}")]
        public IActionResult StatusCodeHandler(int code)
        {
            if (code == 404)
            {
                Response.StatusCode = 404;

                return View("NotFound");
            }

            Response.StatusCode = code;
            ViewData["StatusCode"] = code;

            return View("StatusCode");
        }
    }
}