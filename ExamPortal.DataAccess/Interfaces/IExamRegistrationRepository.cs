using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces
{
    public interface IExamRegistrationRepository : IGenericRepository<ExamRegistration>
    {
        Task<bool> CheckAlreadyRegisteredForExamAsync(int examId, int userId);

        Task<List<ExamRegistration>> GetUpcomingRegistrationsByUserIdAsync(int userId);
        Task<int> GetRegistrationCountByExamIdAsync(int examId);
    }
}