using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        public Task<Exam> GetExamByNameAsync(string name);

        public Task<IEnumerable<Exam>> GetAllAsync();

        Task<List<Exam>> GetRecentlyCreatedExamsAsync(int count = 5);

        Task<int> GetTotalExamCountAsync();
        Task<int> GetActiveExamCountAsync();
        Task<List<Exam>> GetUpcomingExamsAsync(int take = 5);
    }
}