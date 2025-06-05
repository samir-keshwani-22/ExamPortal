using ExamPortal.BusinessLogic.ViewModel.Account;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IAccountService
{
    public Task<bool> RegisterAsync(RegistrationViewModel model);
    public Task<string> LoginAsync(LoginViewModel model);


}
