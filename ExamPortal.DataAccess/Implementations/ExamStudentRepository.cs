using ExamPortal.DataAccess.DataContext;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.Implementations
{
    public class ExamStudentRepository : GenericRepository<ExamStudent>, IExamStudentRepository
    {
        private readonly ExamPortalContext _examPortalContext;
        public ExamStudentRepository(ExamPortalContext examPortalContext) : base(examPortalContext)
        {
            _examPortalContext = examPortalContext;
        }

        public async Task<List<ExamStudent>> GetAssignedStudentAsync(int examId)
        {
            return await _examPortalContext.ExamStudents.Where
            (es => es.ExamId == examId).ToListAsync();
        }

        public async Task UpdateAssignedStudentsAsync(int examId, List<int> selectedStudentIds)
        {
            var existingAssignments = await _examPortalContext.ExamStudents.Where(es => es.ExamId == examId).ToListAsync();
            var existingStudentIds = existingAssignments.Select(es => es.StudentId).ToList();
            var toRemove = existingAssignments.Where(es => !selectedStudentIds.Contains(es.StudentId)).ToList();
            var toAdd = selectedStudentIds.Where(s => !existingStudentIds.Contains(s)).Select(sid => new ExamStudent
            {
                ExamId = examId,
                StudentId = sid
            }).ToList();
            if (toRemove.Any())
            {
                _examPortalContext.ExamStudents.RemoveRange(toRemove);
            }
            if (toAdd.Any())
            {
                await _examPortalContext.ExamStudents.AddRangeAsync(toAdd);
            }
            await _examPortalContext.SaveChangesAsync();
        }
    }
}