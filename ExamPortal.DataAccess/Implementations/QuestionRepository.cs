using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{

    private readonly ExamPortalContext _examPortalContext;
    public QuestionRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
    {
        _examPortalContext = examPortalContext;
    }

    public async Task<List<Question>> GetQuestionsByExamIdAsync(int examId)
    {
        return await _examPortalContext.Questions.Where(q => q.ExamId == examId && q.IsDeleted == false).ToListAsync();
    }

}
