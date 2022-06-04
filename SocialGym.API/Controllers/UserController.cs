using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialGym.BLL.Interfaces;
using SocialGym.BLL.Models;

namespace SocialGym.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;

    public UserController
    (
        IUsersRepository usersRepository
    )
    {
        _usersRepository = usersRepository;
    }

    // GET: api/users/string
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
            BackSquatPR = user.BackSquatPR,
            BenchPressPR = user.BenchPressPR,
            DeadLiftPR = user.DeadLiftPR,
        };
    }

    // DELETE: api/users/string
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
