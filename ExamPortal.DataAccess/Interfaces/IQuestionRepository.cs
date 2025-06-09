using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces;

public interface IQuestionRepository : IGenericRepository<Question>
{
    public Task<List<Question>> GetQuestionsByExamIdAsync(int examId);
}
