using ExamPortal.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;
        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _adminDashboardService.GetDashboardDataAsync();
            return View(model);
        }
        
    }
}