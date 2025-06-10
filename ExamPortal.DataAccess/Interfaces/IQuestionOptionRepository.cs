using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces
{
    public interface IQuestionOptionRepository : IGenericRepository<QuestionOption>
    {
        public Task<List<QuestionOption>> GetOptionsByQuestionIdAsync(int questionId);
    }
}