using ExamPortal.BusinessLogic.ViewModel.StudentDashboard;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IStudentDashboardService
{
    Task<StudentDashboardViewModel> GetDashboardAsync(string studentEmail);
}
