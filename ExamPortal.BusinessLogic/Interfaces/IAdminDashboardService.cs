using ExamPortal.BusinessLogic.ViewModel.AdminDashboard;

namespace ExamPortal.BusinessLogic.Interfaces
{
    public interface IAdminDashboardService
    {
        public Task<AdminDashboardViewModel> GetDashboardDataAsync();
    }
}