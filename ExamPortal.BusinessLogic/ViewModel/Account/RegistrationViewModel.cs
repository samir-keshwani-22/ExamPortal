using System.ComponentModel.DataAnnotations;

namespace ExamPortal.BusinessLogic.ViewModel.Account
{
    public class RegistrationViewModel : LoginViewModel
    {
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "First Name can only contain letters, spaces, apostrophes, or hyphens.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "Last Name can only contain letters, spaces, apostrophes, or hyphens.")]
        public string LastName { get; set; } = null!;

    }
}