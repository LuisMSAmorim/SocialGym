using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;
using SocialGym.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace SocialGym.DAL.Repositories;

public sealed class CommunitiesRepository : ICommunitiesRepository
{
    private readonly SocialGymDbContext _context;

    public CommunitiesRepository
    (
        SocialGymDbContext context
    )
    {
        _context = context;
    }

    public async Task AddAsync(Community community)
    {
        _context.Add(community);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Community community)
    {
        _context.Remove(community);
        await _context.SaveChangesAsync();
    }

    public async Task<CommunityParticipant> GetAdminByCommunityIdAsync(int id)
    {
        return await _context.CommunityParticipant
            .Where(x => x.CommunityId == id)
            .FirstOrDefaultAsync(x => x.IsAdmin == true);
    }

    public async Task<List<Community>> GetUserCommunitiesAsync(string userId)
    {
        return await _context.Community
            .SelectMany(x => x.Participants)
            .Where(x => x.UserId == userId)
            .OfType<Community>()
            .ToListAsync();            
    }

    public async Task<List<CommunityParticipant>> GetAllParticipantsByCommunityIdAsync(int id)
    {
        return await _context.CommunityParticipant
            .Where(x => x.CommunityId == id)
            .Include(x => x.Community)
            .ToListAsync();
    }

    public async Task<Community> GetByIdAsync(int id)
    {
        return await _context.Community.FirstOrDefaultAsync(x => x.CommunityId == id);
    }

    public async Task UpdateAsync(int id, CommunityDTO communityData)
    {
        var community = await _context.Community.SingleOrDefaultAsync(x => x.CommunityId == id);

        _context.Entry(community).CurrentValues.SetValues(communityData);

        await _context.SaveChangesAsync();
    }
}
