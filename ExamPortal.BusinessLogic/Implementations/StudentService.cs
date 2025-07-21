using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Student;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;

namespace ExamPortal.BusinessLogic.Implementations;

public class StudentService : IStudentService
{
    private readonly IUserRepository _userRepository;
    public StudentService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> AddStudent(StudentViewModel model)
    {
        User user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Address = model.Address,
            Zipcode = model.Zipcode,
            MobileNumber = model.Phonenumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            RoleId = 1
        };
        await _userRepository.AddAsync(user);
        return true;
    }

    public async Task<bool> CheckStudentExistsAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user != null)
            return true;
        return false;
    }

    public async Task<bool> CheckStudentExistsForEditAsync(string email, int studentId)
    {
        User user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return false;
        if (studentId == user.Id)
            return false;
        return true;
    }


    public async Task<bool> DeleteStudentAsync(int studentId)
    {
        User student = await _userRepository.GetByIdAsync(studentId);
        if (student == null)
            return false;
        student.IsDeleted = true;
        return await _userRepository.UpdateAsync(student);
    }

    public async Task<bool> EditStudentAsync(EditStudentViewModel model)
    {
        User student = await _userRepository.GetByIdAsync(model.Id);
        student.FirstName = model.FirstName;
        student.LastName = model.LastName;
        student.MobileNumber = model.Phonenumber;
        student.Zipcode = model.Zipcode;
        student.Address = model.Address;
        return await _userRepository.UpdateAsync(student);
    }


    public async Task<List<StudentViewModel>> GetAllStudentAsync(string sortBy = "FirstName", bool ascending = true)
    {
        List<User> user = await _userRepository.GetAllStudents(sortBy, ascending);
        return user.Select(student => new StudentViewModel
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Phonenumber = student.MobileNumber,
            ProfileImage = student.ProfileImg
        }).ToList();
    }

    public async Task<EditStudentViewModel> GetEditStudentModal(int studentId)
    {
        User student = await _userRepository.GetByIdAsync(studentId);
        return student == null ? new EditStudentViewModel() : new EditStudentViewModel
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Phonenumber = student.MobileNumber,
            Zipcode = student.Zipcode,
            Address = student.Address,

        };
    }



}
