using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        public Task<Exam> GetExamByNameAsync(string name);
    }
}