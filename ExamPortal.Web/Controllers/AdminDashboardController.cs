using ExamPortal.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.Web.Controllers
{
    /// <summary>
    /// Controller for admin dashboard operation
    /// </summary>
    public class AdminDashboardController : Controller
    {
        #region Fields 
        private readonly IAdminDashboardService _adminDashboardService;

        #endregion

        #region Constructors 
        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the admin dashboard view with data 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var model = await _adminDashboardService.GetDashboardDataAsync();
            return View(model);
        }

        #endregion

    }
}