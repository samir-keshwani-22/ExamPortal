using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<List<Feedback>> GetRecentFeedbacksAsync(int count = 5);
    }
}