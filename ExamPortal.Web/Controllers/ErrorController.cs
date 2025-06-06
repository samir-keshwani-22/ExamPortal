using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;


[Route("[controller]")]
public class ErrorController : Controller
{

    [HttpGet("AccessDeniedPage")]
    public IActionResult AccessDeniedPage()
    {
        return View();
    }
}
