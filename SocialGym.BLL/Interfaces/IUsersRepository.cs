using SocialGym.BLL.Entities;
using SocialGym.BLL.Models;

namespace SocialGym.BLL.Interfaces;

public interface IUsersRepository
{
    Task AddAsync(User user);
    Task DeleteAsync(User user);
    Task UpdateProfileAsync(int id, UserProfile profile);
    Task<User> GetByIdAsync(int id);
    Task<UserProfile> GetUserProfileAsync(int userId);
}
