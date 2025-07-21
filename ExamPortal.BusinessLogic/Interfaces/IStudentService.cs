using ExamPortal.BusinessLogic.ViewModel.Student;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IStudentService
{
    Task<List<StudentViewModel>> GetAllStudentAsync(string sortBy = "FirstName", bool ascending = true);

    Task<bool> CheckStudentExistsAsync(string email);
    Task<bool> CheckStudentExistsForEditAsync(string email, int studentId);

    Task<bool> AddStudent(StudentViewModel model);

    Task<bool> DeleteStudentAsync(int studentId);

    Task<EditStudentViewModel> GetEditStudentModal(int studentId);

    Task<bool> EditStudentAsync(EditStudentViewModel model);



}
