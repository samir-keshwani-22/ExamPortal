using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Account;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Npgsql.Internal.TypeMapping;

namespace ExamPortal.BusinessLogic.Implementations;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;


    public AccountService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;

    }

    public async Task<bool> RegisterAsync(RegistrationViewModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), "Registration model cannot be null ");

        }
        var existingUsers = await _userRepository.GetByEmailAsync(model.Email);

        if (existingUsers != null)
        {

            return false;
        }
        var user = new User
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            RoleId = 1
        };
        await _userRepository.AddAsync(user);
        return true;
    }

    public async Task<string> LoginAsync(LoginViewModel model)
    {
        var user = await _userRepository.GetByEmailAsync(model.Email);
        if (user == null || (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash)))
        {
            return null;
        }
        var token = _jwtService.GenerateToken(user.Email, user.Role.Name, model.RememberMe);
        return token;
    }  
}
