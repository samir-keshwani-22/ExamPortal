using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExamPortal.BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExamPortal.BusinessLogic.Implementations;

public class JwtService : IJwtService
{

    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(string email, string role = null, bool rememberMe = false, bool isPasswordReset = false)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
        if (!isPasswordReset && role != null)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = isPasswordReset ? DateTime.Now.AddHours(24) : (rememberMe ? DateTime.Now.AddDays(15) : DateTime.Now.AddHours(1)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch (SecurityTokenExpiredException)
        {

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

}
