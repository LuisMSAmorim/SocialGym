using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;
using SocialGym.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace SocialGym.DAL.Repositories;

public sealed class PostsRepository : IPostsRepository
{
    private readonly SocialGymDbContext _context;

    public PostsRepository
    (
        SocialGymDbContext context
    )
    {
        _context = context;
    }

    public async Task AddAsync(Post post)
    {
        _context.Add(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Post post)
    {
        _context.Remove(post);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Post>> GetAllByCommunityIdAsync(int communityId)
    {
        return await _context.Post
            .Where(x => x.CommunityParticipant.CommunityId == communityId)
            .Include(x => x.CommunityParticipant.Community)
            .ToListAsync();
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        return await _context.Post
            .Include(x => x.CommunityParticipant)
            .FirstOrDefaultAsync(x => x.PostId == id);
    }
}
