using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;

namespace SocialGym.BLL.Interfaces;

public interface ICommunitiesRepository
{
    Task AddAsync(User user, Community community);

    Task AddParticipantAsync(User user, Community community);
    Task<List<Community>> GetAllAsync();
    Task DeleteAsync(Community community);
    Task UpdateAsync(int id, CommunityDTO community);
    Task<Community> GetByIdAsync(int id);
    Task<Community> GetByNameAsync(string name);
    Task<List<Community>> GetUserCommunitiesAsync(string userName);
    Task<List<CommunityParticipant>> GetAllParticipantsByCommunityIdAsync(int id);
    Task<CommunityParticipant> GetParticipantByUserIdAndCommunityId(string userId, int communityId);
    Task<CommunityParticipant> GetAdminByCommunityIdAsync(int id);
}
