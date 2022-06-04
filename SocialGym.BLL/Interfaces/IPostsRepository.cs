using SocialGym.BLL.Entities;

namespace SocialGym.BLL.Interfaces;

public interface IPostsRepository
{
    Task AddAsync(Post post);
    Task DeleteAsync(Post post);
    Task<Post> GetByIdAsync(int id);
    Task<List<Post>> GetAllByCommunityId(int communityId);
}
