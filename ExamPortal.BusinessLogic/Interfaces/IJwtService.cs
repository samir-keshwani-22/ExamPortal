using System.Security.Claims;

namespace ExamPortal.BusinessLogic.Interfaces;

public interface IJwtService
{
    string GenerateToken(string email, string role = null, bool rememberMe = false, bool isPasswordReset = false);
    ClaimsPrincipal ValidateToken(string token);
}
