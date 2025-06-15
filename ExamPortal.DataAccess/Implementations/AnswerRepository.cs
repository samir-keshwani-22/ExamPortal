using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations
{
    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        private readonly ExamPortalContext _examPortalContext;
        public AnswerRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
        {
            _examPortalContext = examPortalContext;
        }

        public async Task<Answer> GetAnswerAsync(int attemptId, int questionId)
        {
            return await _examPortalContext.Answers.FirstOrDefaultAsync(a => a.AttemptId == attemptId && a.QuestionId == questionId);

        }

    }
}