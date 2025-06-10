using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations
{
    public class QuestionOptionRepository : GenericRepository<QuestionOption>, IQuestionOptionRepository
    {
        private readonly ExamPortalContext _examPortalContext;
        public QuestionOptionRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
        {
            _examPortalContext = examPortalContext;
        }

        public async Task<List<QuestionOption>> GetOptionsByQuestionIdAsync(int questionId)
        {
            return await _examPortalContext.QuestionOptions.Where(q => q.QuestionId == questionId).ToListAsync();
        }

    }
}