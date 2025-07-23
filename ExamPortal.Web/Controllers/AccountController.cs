using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;
/// <summary>
/// Controller for the account related operation
/// </summary>
[AllowAnonymous]
public class AccountController : Controller
{
    #region  Fields
    private readonly IAccountService _accountService;

    #endregion 

    #region  Constructors
    /// <summary>
    /// AccountController constructor
    /// </summary>
    /// <param name="accountService"></param>
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    #endregion

    #region Methods 

    /// <summary>
    /// Returns the login view if the user not auth else redirects to the home page
    /// </summary>
    /// <returns></returns>

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

    /// <summary>
    /// Handles the login operation.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns the registration view
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    /// <summary>
    /// Handles the registration operation
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>

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

    /// <summary>
    /// Handles the logout operation
    /// </summary>
    /// <returns></returns>

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        TempData["SuccessMessage"] = "Logged out successfully.";
        return RedirectToAction("Login");
    }

    /// <summary>
    /// Returns the view for the forget password 
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public IActionResult ForgetPassword()
    {
        return View();
    }

    /// <summary>
    /// Handles the forget password operation 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns the view for resetting the password 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        return View(new ResetPasswordViewModel { Token = token });
    }

    /// <summary>
    /// Handles the reset password operation
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
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

    #endregion
}
