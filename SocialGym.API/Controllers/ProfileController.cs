using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialGym.BLL.Interfaces;
using SocialGym.BLL.Models;

namespace SocialGym.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;
    private readonly ICommunitiesRepository _communitiesRepository;

    public ProfileController
    (
        IUsersRepository usersRepository,
        ICommunitiesRepository communitiesRepository
    )
    {
        _usersRepository = usersRepository;
        _communitiesRepository = communitiesRepository;
    }

    // GET: api/profiles/string
    [HttpGet("{userName}")]
    public async Task<ActionResult<UserProfile>> GetProfile(string userName)
    {
        var user = await _usersRepository.FindByNameAsync(userName);

        if (user == null)
        {
            return NotFound();
        }

        var communities = await _communitiesRepository.GetUserCommunitiesAsync(user.Id);

        return new UserProfile()
        {
            UserName = user.UserName,
            Avatar = user.Avatar,
            BackSquatPR = user.BackSquatPR,
            BenchPressPR = user.BenchPressPR,
            DeadLiftPR = user.DeadLiftPR,
            Communities = communities
        };
    }
}
