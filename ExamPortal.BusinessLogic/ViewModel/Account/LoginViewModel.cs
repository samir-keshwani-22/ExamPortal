using System.ComponentModel.DataAnnotations;

namespace ExamPortal.BusinessLogic.ViewModel.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
}