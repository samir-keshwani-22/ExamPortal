using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {

        private readonly ExamPortalContext _examPortalContext;
        public ExamRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
        {
            _examPortalContext = examPortalContext;
        }

        public async Task<Exam> GetExamByNameAsync(string name)
        {
            return await _examPortalContext.Exams.FirstOrDefaultAsync(e => e.Title.ToLower() == name.ToLower());
        }

    }
}