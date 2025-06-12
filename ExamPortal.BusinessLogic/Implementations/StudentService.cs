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
    public async Task<List<StudentViewModel>> GetAllStudentAsync()
    {
        List<User> user = await _userRepository.GetAllStudents();
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
 
}
