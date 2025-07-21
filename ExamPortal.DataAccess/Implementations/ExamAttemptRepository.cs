using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations;

public class ExamAttemptRepository : GenericRepository<ExamAttempt>, IExamAttemptRepository
{
    private readonly ExamPortalContext _examPortalContext;
    public ExamAttemptRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
    {
        _examPortalContext = examPortalContext;
    }

    public async Task<bool> CheckIfAlreadyAttemptedAsync(int examId, int userId)
    {
        return await _examPortalContext.ExamAttempts
            .AnyAsync(a => a.ExamId == examId && a.UserId == userId);
    }

    public async Task<ExamAttempt?> GetAttemptWithDetailsAsync(int attemptId)
    {
        return await _examPortalContext.ExamAttempts.
        Include(e => e.Exam)
        .Include(a => a.Answers)
            .ThenInclude(q => q.Question)
                .ThenInclude(o => o.Options)
        .Include(a => a.Answers)
            .ThenInclude(ans => ans.SelectedOption)
            .FirstOrDefaultAsync(a => a.Id == attemptId);
    }

    public async Task<List<ExamAttempt>> GetByUserIdAsync(int userId)
    {
        return await _examPortalContext.ExamAttempts.Where(a => a.UserId == userId).Include(a => a.Exam).ToListAsync();
    }

    public async Task<int> GetTotalAttemptCountAsync()
    {
        return await _examPortalContext.ExamAttempts.CountAsync();
    }
}
