using System.ComponentModel.DataAnnotations;

namespace ExamPortal.BusinessLogic.ViewModel.Account
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;
    }
}