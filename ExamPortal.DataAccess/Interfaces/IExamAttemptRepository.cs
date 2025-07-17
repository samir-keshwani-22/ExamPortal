using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces;

public interface IExamAttemptRepository : IGenericRepository<ExamAttempt>
{
       Task<bool> CheckIfAlreadyAttemptedAsync(int examId, int userId);
       Task<ExamAttempt?> GetAttemptWithDetailsAsync(int attemptId);

       Task<List<ExamAttempt>> GetByUserIdAsync(int userId);
       

}
