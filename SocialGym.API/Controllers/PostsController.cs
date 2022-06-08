using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;
using SocialGym.BLL.ViewModels;

namespace SocialGym.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostsRepository _postsRepository;
    private readonly ICommunitiesRepository _communitiesRepository;
    private readonly IUsersRepository _usersRepository;

    public PostsController
    (
        IPostsRepository postsRepository,
        ICommunitiesRepository communitiesRepository,
        IUsersRepository usersRepository
    )
    {
        _postsRepository = postsRepository;
        _communitiesRepository = communitiesRepository;
        _usersRepository = usersRepository;

    }

    // POST: api/posts/5
    [HttpPost("{communityId}")]
    [Authorize]
    public async Task<ActionResult> CreatePost(int communityId, PostDTO postDTO)
    {
        var community = await _communitiesRepository.GetByIdAsync(communityId);

        if(community == null)
        {
            return NotFound("Community Not Found");
        }

        var userClaims = User.Claims.FirstOrDefault();

        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        var communityParticipant = await _communitiesRepository.GetParticipantByUserIdAndCommunityId(user.Id, community.CommunityId);

        if(communityParticipant == null)
        {
            return BadRequest("User Isnt Community Participant");
        }

        Post post = new()
        {
            Title = postDTO.Title,
            Text = postDTO.Text,
            ImageUrl = postDTO.ImageUrl,
            CreatedAt = DateTime.Now,
            CommunityParticipant = communityParticipant,
            CommunityParticipantId = communityParticipant.CommunityId,
        };

        await _postsRepository.AddAsync(post);

        return Ok(new { Message = "Post Created Successfully" });
    }

    // GET: api/posts/communities/5
    [HttpGet]
    [Route("communities/{communityId}")]
    [Authorize]
    public async Task<ActionResult<List<PostViewModel>>> GetCommunityPosts(int communityId)
    {
        var community = await _communitiesRepository.GetByIdAsync(communityId);

        if (community == null)
        {
            return NotFound("Community Not Found");
        }

        var userClaims = User.Claims.FirstOrDefault();

        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        var communityParticipant = await _communitiesRepository.GetParticipantByUserIdAndCommunityId(user.Id, community.CommunityId);

        if (communityParticipant == null)
        {
            return BadRequest("User Isnt Community Participant");
        }

        var posts = await _postsRepository.GetAllByCommunityIdAsync(communityId);

        return posts
            .Select(x => new PostViewModel()
            {
                CommunityParticipantUserName = x.CommunityParticipant.User.UserName,
                CommunityParticipantAvatar = x.CommunityParticipant.User.Avatar,
                CreatedAt = x.CreatedAt,
                ImageUrl = x.ImageUrl,
                PostId = x.PostId,
                Text = x.Text,
                Title = x.Title
            })
            .ToList();
    }

    // GET: api/posts/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<PostViewModel>> GetCommunityPost(int id)
    {
        var post = await _postsRepository.GetByIdAsync(id);

        if(post == null)
        {
            return NotFound();
        }

        var community = await _communitiesRepository.GetByIdAsync(post.CommunityParticipant.CommunityId);

        if (community == null)
        {
            return NotFound("Community Not Found");
        }

        var userClaims = User.Claims.FirstOrDefault();

        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        var communityParticipant = await _communitiesRepository.GetParticipantByUserIdAndCommunityId(user.Id, community.CommunityId);

        if (communityParticipant == null)
        {
            return BadRequest("User Isnt Community Participant");
        }

        return new PostViewModel()
        {
            CommunityParticipantUserName = post.CommunityParticipant.User.UserName,
            CommunityParticipantAvatar = post.CommunityParticipant.User.Avatar,
            CommunityId = post.CommunityParticipant.CommunityId,
            CreatedAt = post.CreatedAt,
            ImageUrl = post.ImageUrl,
            PostId = post.PostId,
            Text = post.Text,
            Title = post.Title
        };
    }

    // DELETE: api/posts/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteCommunityPost(int id)
    {
        var post = await _postsRepository.GetByIdAsync(id);

        var postAuthorId = post.CommunityParticipant.UserId;

        if (post == null)
        {
            return NotFound();
        }

        var community = await _communitiesRepository.GetByIdAsync(post.CommunityParticipant.CommunityId);

        if (community == null)
        {
            return NotFound("Community Not Found");
        }

        var admin = await _communitiesRepository.GetAdminByCommunityIdAsync(community.CommunityId);
        var adminId = admin.UserId;

        var userClaims = User.Claims.FirstOrDefault();

        var user = await _usersRepository.GetByNameAsync(userClaims.Subject.Name);

        if (postAuthorId != user.Id && adminId != user.Id)
        {
            return BadRequest("User Isnt the post author or community admin");
        }

        await _postsRepository.DeleteAsync(post);

        return NoContent();
    }
}
