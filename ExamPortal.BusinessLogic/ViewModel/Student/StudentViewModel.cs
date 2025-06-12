using System.ComponentModel.DataAnnotations;
using ExamPortal.BusinessLogic.ViewModel.Profile;

namespace ExamPortal.BusinessLogic.ViewModel.Student;

public class StudentViewModel : MyProfileViewModel
{
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
          ErrorMessage = "Password must be between 8 and 15 characters and contain one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string Password { get; set; }
}
