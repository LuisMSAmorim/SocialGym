using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;

namespace SocialGym.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommunitiesController : ControllerBase
{
    private readonly ICommunitiesRepository _communitiesRepository;
    private readonly IUsersRepository _usersRepository;

    public CommunitiesController
    (
        ICommunitiesRepository communitiesRepository,
        IUsersRepository usersRepository
    )
    {
        _communitiesRepository = communitiesRepository;
        _usersRepository = usersRepository;
    }

    // POST: api/communities
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateCommunity(CommunityDTO communityDTO)
    {
        var communtityAlreadyExists = await _communitiesRepository.GetByNameAsync(communityDTO.Name);

        if (communtityAlreadyExists != null)
        {
            return BadRequest("Name already exists");
        }

        var userClaims = User.Claims.FirstOrDefault();

        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        Community community = new()
        {
            Name = communityDTO.Name,
            Description = communityDTO.Description,
        };

        await _communitiesRepository.AddAsync(user, community);

        return Ok(new { Message = "Community Created Successfully" });
    }
}
