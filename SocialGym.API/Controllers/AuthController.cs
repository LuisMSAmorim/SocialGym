using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialGym.API.Config;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;
using SocialGym.BLL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialGym.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
    private readonly IUsersRepository usersRepository;
    private readonly UserManager<User> userManager;

    public AuthController
    (
        IOptions<JwtBearerTokenSettings> jwtTokenOptions,
        IUsersRepository usersRepository,
        UserManager<User> userManager
    )
    {
        this.jwtBearerTokenSettings = jwtTokenOptions.Value;
        this.usersRepository = usersRepository;
        this.userManager = userManager;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
    {
        User user;

        if (!ModelState.IsValid
            || credentials == null
            || (user = await ValidateUser(credentials)) == null)
        {
            return new BadRequestObjectResult(new { Message = "Login failed" });
        }

        var token = GenerateToken(user);
        return Ok(new { Token = token, Message = "Success" });
    }

    private async Task<User> ValidateUser(LoginCredentials credentials)
    {
        var user = await usersRepository.GetByNameAsync(credentials.Username);
        if (user != null)
        {
            var result = userManager.PasswordHasher
                .VerifyHashedPassword(user, user.PasswordHash, credentials.Password);

            return result == PasswordVerificationResult.Failed ? null : user;
        }

        return null;
    }

    private object GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
            }),

            Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = jwtBearerTokenSettings.Audience,
            Issuer = jwtBearerTokenSettings.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
