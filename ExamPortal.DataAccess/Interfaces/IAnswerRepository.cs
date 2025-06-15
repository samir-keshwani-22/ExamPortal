using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces
{
    public interface IAnswerRepository: IGenericRepository<Answer>
    {
        Task<Answer> GetAnswerAsync(int attemptId, int questionId);


    }
}