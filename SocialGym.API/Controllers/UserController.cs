using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;

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

    // GET: api/users/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        var user = await _usersRepository.GetByIdAsync(id);

        var claims = User.Claims.FirstOrDefault(); 

        if (claims.Subject.Name != user.UserName)
        {
            return BadRequest();
        };

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _usersRepository.GetByIdAsync(id);

        var claims = User.Claims.FirstOrDefault();

        if (claims.Subject.Name != user.UserName)
        {
            return BadRequest();
        };

        if (user == null)
        {
            return NotFound();
        }

        await _usersRepository.DeleteAsync(user);

        return NoContent();
    }
}
