using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialGym.BLL.Interfaces;
using SocialGym.BLL.ViewModels;

namespace SocialGym.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfilesController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;

    public ProfilesController
    (
        IUsersRepository usersRepository
    )
    {
        _usersRepository = usersRepository;
    }

    // GET: api/profiles/string
    [HttpGet("{userName}")]
    public async Task<ActionResult<UserProfileViewModel>> GetProfile(string userName)
    {
        var user = await _usersRepository.GetByNameAsync(userName);

        if (user == null)
        {
            return NotFound();
        }

        return new UserProfileViewModel()
        {
            UserName = user.UserName,
            Avatar = user.Avatar,
            BackSquatPR = user.BackSquatPR,
            BenchPressPR = user.BenchPressPR,
            DeadLiftPR = user.DeadLiftPR,
        };
    }

    // PUT: api/profiles/string
    [HttpPut("{userName}")]
    [Authorize]
    public async Task<IActionResult> UpdateAccount(string userName, UserProfileViewModel profile)
    {
        var user = await _usersRepository.GetByNameAsync(userName);

        if (user == null)
        {
            return NotFound();
        }

        var claims = User.Claims.FirstOrDefault();

        if (claims.Subject.Name != user.UserName)
        {
            return Unauthorized();
        };

        var result = await _usersRepository.UpdateProfileAsync(user.Id, profile);

        if (!result.Succeeded)
        {
            var dictionary = new ModelStateDictionary();
            foreach (IdentityError error in result.Errors)
            {
                dictionary.AddModelError(error.Code, error.Description);
            }

            return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
        }

        return Ok(new { Message = "Profile Updated Successful" });
    }
}
