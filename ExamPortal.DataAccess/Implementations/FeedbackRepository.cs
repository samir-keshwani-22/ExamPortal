using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly ExamPortalContext _examPortalContext;
        public FeedbackRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
        {
            _examPortalContext = examPortalContext;
        }

        public async Task<List<Feedback>> GetRecentFeedbacksAsync(int count = 5)
        {
            return await _examPortalContext.Feedbacks
               .Include(f => f.User)
               .OrderByDescending(f => f.CreatedAt)
               .Take(count)
               .ToListAsync();
        }
    }
}