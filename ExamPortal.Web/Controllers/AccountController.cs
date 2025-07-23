using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Login()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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
        if (ModelState.IsValid) TempData["SuccessMessage"] = "Logged in Successfully";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        bool result = await _accountService.RegisterAsync(model);
        if (result)
        {
            TempData["SuccessMessage"] = "Registration successful.You can now log in.";
            return RedirectToAction("Login");
        }
        ModelState.AddModelError("", "Registration failed. Please try again.");
        return View(model);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        TempData["SuccessMessage"] = "Logged out successfully.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ForgetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Failed to reset the password";
            return View(model);
        }
        bool success = await _accountService.ForgetPasswordAsync(model.Email, Url);
        if (!success)
        {
            TempData["ErrorMessage"] = "Failed to reset the password";
            ModelState.AddModelError(string.Empty, "No account found with this email.");
            return View(model);
        }
        TempData["SuccessMessage"] = "A Password reset link has been sent to your email.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        return View(new ResetPasswordViewModel { Token = token });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            bool success = await _accountService.ResetPasswordAsync(model);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to reset the password";
                ModelState.AddModelError(string.Empty, "Invalid Token");
                return View(model);
            }
            TempData["SuccessMessage"] = "Password has been reset successfully.";
            return RedirectToAction("Login");
        }
        return View(model);
    }
}
