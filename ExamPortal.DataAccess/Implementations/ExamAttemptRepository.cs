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
}
