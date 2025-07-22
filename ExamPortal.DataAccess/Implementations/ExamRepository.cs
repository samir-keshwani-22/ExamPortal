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
            return await _examPortalContext.Exams.Where(e => e.IsDeleted == false).OrderByDescending(e => e.StartDate).ToListAsync();
        }

        public async Task<int> GetTotalExamCountAsync()
        {
            return await _examPortalContext.Exams.CountAsync(e => e.IsDeleted == false);
        }

        public async Task<int> GetActiveExamCountAsync()
        {
            return await _examPortalContext.Exams.CountAsync(e => e.IsDeleted == false && e.StartDate < DateTime.Now && e.EndDate > DateTime.Now);
        }

        public async Task<List<Exam>> GetUpcomingExamsAsync(int take = 5)
        {
            return await _examPortalContext.Exams.Where(e => e.StartDate > DateTime.Now).OrderBy(e => e.StartDate).Take(take).ToListAsync();
        }

        public async Task<List<Exam>> GetRecentlyCreatedExamsAsync(int count = 5)
        {
            return await _examPortalContext.Exams
                .Where(e => !e.IsDeleted.Value)
                .OrderByDescending(e => e.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsForStudentAsync(int studentId)
        {
            return await _examPortalContext.ExamStudents
            .Where(es => es.StudentId == studentId)
            .Select(es => es.Exam)
            .Where(e => e.IsDeleted == false)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
        }

    }
}