using System.Security.Claims;
using ExamPortal.BusinessLogic.Interfaces;
using ExamPortal.BusinessLogic.ViewModel.Account;
using ExamPortal.DataAccess.Interfaces;
using ExamPortal.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamPortal.BusinessLogic.Implementations;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    public AccountService(IUserRepository userRepository, IJwtService jwtService, IEmailService emailService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    public async Task<bool> RegisterAsync(RegistrationViewModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model), "Registration model cannot be null ");
        }
        User existingUsers = await _userRepository.GetByEmailAsync(model.Email);
        if (existingUsers != null)
        {
            return false;
        }
        User user = new User
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
        User user = await _userRepository.GetByEmailAsync(model.Email);
        if (user == null || (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash)))
        {
            return null;
        }
        string token = _jwtService.GenerateToken(user.Email, user.Role.Name, model.RememberMe);
        return token;
    }

    public async Task<bool> ForgetPasswordAsync(string email, IUrlHelper urlHelper)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        string resetToken = _jwtService.GenerateToken(user.Email, isPasswordReset: true);
        user.ResetToken = resetToken;
        await _userRepository.UpdateAsync(user);
        string resetLink = urlHelper.Action("ResetPassword", "Account", new { token = resetToken }, "http");
        _emailService.SendEmail(email, resetLink);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordViewModel model)
    {
        User user = await _userRepository.GetByResetToken(model.Token);
        ClaimsPrincipal principal = _jwtService.ValidateToken(model.Token);
        if (principal == null)
        {
            return false;
        }
        if (user == null)
        {
            return false;
        }
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
        user.ResetToken = null;
        await _userRepository.UpdateAsync(user);
        return true;
    }
}
