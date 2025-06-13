using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations
{
    public class ExamRegistrationRepository : GenericRepository<ExamRegistration>, IExamRegistrationRepository
    {
        private readonly ExamPortalContext _examPortalContext;
        public ExamRegistrationRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
        {
            _examPortalContext = examPortalContext;
        }

        public async Task<bool> CheckAlreadyRegisteredForExamAsync(int examId, int userId)
        {
            return await _examPortalContext.ExamRegistrations.AnyAsync(er => er.ExamId == examId && er.UserId == userId);
        }
    }
}