using ExamPortal.DataAccess.Models;

namespace ExamPortal.DataAccess.Interfaces
{
    public interface IExamStudentRepository : IGenericRepository<ExamStudent>
    {
        Task<List<ExamStudent>> GetAssignedStudentAsync(int examId);
        Task UpdateAssignedStudentsAsync(int examId, List<int> selectedStudentIds);
    }
}