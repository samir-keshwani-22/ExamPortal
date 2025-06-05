using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;

[AllowAnonymous]
[Route("Account")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("Login")]

    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        string token = await _accountService.LoginAsync(model);
        if (string.IsNullOrEmpty(token))
        {
            TempData["ErrorMessage"] = "User is not valid or Password is incorrect.";
            ModelState.AddModelError(string.Empty, "User is not valid or Password is incorrect .");
            return View(model);
        }

        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = model.RememberMe ? DateTime.Now.AddDays(15) : DateTime.Now.AddHours(1)
        });
        TempData["SuccessMessage"] = "Logged in Successfully";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("Register")]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await _accountService.RegisterAsync(model);
        if (result)
        {
            TempData["SuccessMessage"] = "Registration successful.You can now log in.";
            return RedirectToAction("Login");
        }
        ModelState.AddModelError("", "Registration failed. Please try again.");
        return View(model);
    }

}
