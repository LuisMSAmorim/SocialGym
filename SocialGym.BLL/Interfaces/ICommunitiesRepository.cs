using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;

namespace SocialGym.BLL.Interfaces;

public interface ICommunitiesRepository
{
    Task AddAsync(Community community);
    Task DeleteAsync(Community community);
    Task UpdateAsync(int id, CommunityDTO community);
    Task<Community> GetByIdAsync(int id);
    Task<List<CommunityParticipant>> GetAllParticipantsAsync(int id);
    Task<CommunityParticipant> GetAdminAsync(int id);
}
