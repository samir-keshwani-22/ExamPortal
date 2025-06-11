using ExamPortal.BusinessLogic.ViewModel.Account;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IAccountService
{
    Task<bool> RegisterAsync(RegistrationViewModel model);
    Task<string> LoginAsync(LoginViewModel model);
    Task<bool> ForgetPasswordAsync(string email, IUrlHelper urlHelper);
    Task<bool> ResetPasswordAsync(ResetPasswordViewModel model);

}
