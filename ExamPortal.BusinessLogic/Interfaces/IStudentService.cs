using ExamPortal.BusinessLogic.ViewModel.Student;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IStudentService
{
    Task<List<StudentViewModel>> GetAllStudentAsync();

    Task<bool> CheckStudentExistsAsync(string email);

    Task<bool> AddStudent(StudentViewModel model);

}
