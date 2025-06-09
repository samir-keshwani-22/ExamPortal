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
            return await _examPortalContext.Exams.FirstOrDefaultAsync(e => e.Title.ToLower() == name.ToLower() && e.IsDeleted == false);
        }

        public async Task<IEnumerable<Exam>> GetAllAsync()
        {
            return await _examPortalContext.Exams.Where(e => e.IsDeleted == false).ToListAsync();
        }

    }
}