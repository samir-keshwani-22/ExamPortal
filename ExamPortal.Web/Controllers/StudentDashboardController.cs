using System.Security.Claims;
using ExamPortal.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers;

public class StudentDashboardController : Controller
{
    private readonly IStudentDashboardService _studentDashboardService;
    public StudentDashboardController(IStudentDashboardService studentDashboardService)
    {
        _studentDashboardService = studentDashboardService;
    }
    public async Task<IActionResult> Index()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var model = await _studentDashboardService.GetDashboardAsync(email);
        return View(model);
    }
}
