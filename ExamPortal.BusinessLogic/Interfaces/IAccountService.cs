using ExamPortal.BusinessLogic.ViewModel.Account;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IAccountService
{
    public Task<bool> RegisterAsync(RegistrationViewModel model);
    public Task<string> LoginAsync(LoginViewModel model);

    Task<bool> ForgetPasswordAsync(string email, IUrlHelper urlHelper);
    Task<bool> ResetPasswordAsync(ResetPasswordViewModel model);

}
