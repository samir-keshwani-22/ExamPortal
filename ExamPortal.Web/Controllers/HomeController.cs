using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExamPortal.Web.Models;
using System.Security.Claims;
namespace ExamPortal.Web.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        return role switch
        {
            "admin" => RedirectToAction("Index", "AdminDashboard"),
            "student" => RedirectToAction("Index", "StudentDashboard"),
            _ => RedirectToAction("AccessDenied", "Account")
        };
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
