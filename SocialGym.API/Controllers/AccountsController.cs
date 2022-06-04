using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialGym.BLL.Interfaces;
using SocialGym.BLL.Models;

namespace SocialGym.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;

    public AccountsController
    (
        IUsersRepository usersRepository
    )
    {
        _usersRepository = usersRepository;
    }

    // POST: api/accounts
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserDetails userDetails)
    {
        if (!ModelState.IsValid || userDetails == null)
        {
            return new BadRequestObjectResult(new { Message = "User Registration Failed" });
        }

        var result = await _usersRepository.AddAsync(userDetails);

        if (!result.Succeeded)
        {
            var dictionary = new ModelStateDictionary();
            foreach (IdentityError error in result.Errors)
            {
                dictionary.AddModelError(error.Code, error.Description);
            }

            return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
        }

        return Ok(new { Message = "User Registration Successful" });
    }

    // GET: api/accounts/string
    [HttpGet("{userName}")]
    [Authorize]
    public async Task<ActionResult<UserAccount>> GetUser(string userName)
    {
        var user = await _usersRepository.FindByNameAsync(userName);

        if (user == null)
        {
            return NotFound();
        }

        var claims = User.Claims.FirstOrDefault(); 

        if (claims.Subject.Name != user.UserName)
        {
            return Unauthorized();
        };

        return new UserAccount()
        {
            Email = user.Email,
            UserName = user.UserName,
            Avatar = user.Avatar,
        };
    }

    // DELETE: api/accounts/string
    [HttpDelete("{userName}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string userName)
    {
        var user = await _usersRepository.FindByNameAsync(userName);

        if (user == null)
        {
            return NotFound();
        }

        var claims = User.Claims.FirstOrDefault();

        if (claims.Subject.Name != user.UserName)
        {
            return Unauthorized();
        };

        await _usersRepository.DeleteAsync(user);

        return NoContent();
    }
}
