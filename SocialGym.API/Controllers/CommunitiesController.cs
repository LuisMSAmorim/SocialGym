using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;
using SocialGym.BLL.ViewModels;

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

    // POST: api/communities/become_participant/5
    [HttpPost]
    [Route("become_participant/{id}")]
    [Authorize]
    public async Task<ActionResult> BecomeCommunityParticipant(int id)
    {
        var community = await _communitiesRepository.GetByIdAsync(id);

        if(community == null)
        {
            return NotFound();
        }

        var userClaims = User.Claims.FirstOrDefault();
        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        var userAlreadyParticipant = await _communitiesRepository.GetParticipantByUserIdAndCommunityId(user.Id, community.CommunityId);

        if (userAlreadyParticipant != null)
        {
            return BadRequest("User already participant");
        }

        await _communitiesRepository.AddParticipantAsync(user, community);

        return Ok(new { Message = "Participation Registration Successful" });
    }

    // GET: api/communities
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Community>>> GetCommunities()
    {
        return await _communitiesRepository.GetAllAsync();
    }

    // GET: api/communities/participants/id
    [HttpGet]
    [Route("participants/{id}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserProfileViewModel>>> GetCommunityParticipants(int id)
    {
        var community = await _communitiesRepository.GetByIdAsync(id);

        if (community == null)
        {
            return NotFound();
        }

        var participants = await _communitiesRepository.GetAllParticipantsByCommunityIdAsync(id);

        if(participants == null)
        {
            return NotFound();
        }

        return participants.Select(x => new UserProfileViewModel()
        {
            UserName = x.User.UserName,
            Avatar = x.User.Avatar,
            BackSquatPR = x.User.BackSquatPR,
            BenchPressPR = x.User.BenchPressPR,
            DeadLiftPR = x.User.DeadLiftPR
        }).ToList();
    }

    // GET: api/communities/admin/id
    [HttpGet]
    [Route("admin/{id}")]
    [Authorize]
    public async Task<ActionResult<UserProfileViewModel>> GetCommunityAdmin(int id)
    {
        var community = await _communitiesRepository.GetByIdAsync(id);

        if (community == null)
        {
            return NotFound();
        }

        var communityAdmin = await _communitiesRepository.GetAdminByCommunityIdAsync(id);

        return new UserProfileViewModel()
        {
            Avatar = communityAdmin.User.Avatar,
            UserName = communityAdmin.User.UserName,
            BackSquatPR = communityAdmin.User.BackSquatPR,
            BenchPressPR = communityAdmin.User.BenchPressPR,
            DeadLiftPR = communityAdmin.User.DeadLiftPR
        };
    }

    // GET: api/communities/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Community>> GetCommunity(int id)
    {
        var community = await _communitiesRepository.GetByIdAsync(id);

        if (community == null)
        {
            return NotFound();
        }

        return community;
    }

    // DELETE: api/communities/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCommunity(int id)
    {
        var community = await _communitiesRepository.GetByIdAsync(id);

        if (community == null)
        {
            return NotFound();
        }

        var userClaims = User.Claims.FirstOrDefault();

        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        var communityAdmin = await _communitiesRepository.GetAdminByCommunityIdAsync(id);

        if (communityAdmin.UserId != user.Id)
        {
            return Unauthorized();
        }

        await _communitiesRepository.DeleteAsync(community);

        return NoContent();
    }

    // PUT: api/communities/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateCommunity(int id, CommunityDTO communityDTO)
    {
        var community = await _communitiesRepository.GetByIdAsync(id);

        if (community == null)
        {
            return NotFound();
        }

        var userClaims = User.Claims.FirstOrDefault();

        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        var communityAdmin = await _communitiesRepository.GetAdminByCommunityIdAsync(id);

        if (communityAdmin.UserId != user.Id)
        {
            return Unauthorized();
        }

        await _communitiesRepository.UpdateAsync(id, communityDTO);

        return NoContent();
    }
}
