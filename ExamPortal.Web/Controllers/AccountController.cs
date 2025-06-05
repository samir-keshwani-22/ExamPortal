using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{

    public IActionResult Login()
    {
        return View();
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View();
    }


}
