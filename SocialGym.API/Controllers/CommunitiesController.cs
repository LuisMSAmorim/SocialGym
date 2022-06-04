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

    // GET: api/communities
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Community>>> GetStyle()
    {
        return await _communitiesRepository.GetAllAsync();
    }

    // GET: api/communities/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Community>> GetStyle(int id)
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

        var communityAdmin = community.Participants.
                                Where(x => x.IsAdmin == true)
                                .FirstOrDefault();

        if(communityAdmin.UserId != user.Id)
        {
            return Unauthorized();
        }

        await _communitiesRepository.DeleteAsync(community);

        return NoContent();
    }

    // PUT: api/communities/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutStyle(int id, CommunityDTO communityDTO)
    {
        var community = await _communitiesRepository.GetByIdAsync(id);

        if (community == null)
        {
            return NotFound();
        }

        var userClaims = User.Claims.FirstOrDefault();

        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        var communityAdmin = community.Participants.
                                Where(x => x.IsAdmin == true)
                                .FirstOrDefault();

        if (communityAdmin.UserId != user.Id)
        {
            return Unauthorized();
        }

        await _communitiesRepository.UpdateAsync(id, communityDTO);

        return NoContent();
    }
}
