using Microsoft.AspNetCore.Identity;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Models;

namespace SocialGym.BLL.Interfaces;

public interface IUsersRepository
{
    Task<IdentityResult> AddAsync(UserDetails userDetails);
    Task DeleteAsync(User user);
    Task<IdentityResult> UpdateProfileAsync(string id, UserProfile profile);
    Task<User> GetByIdAsync(string id);
    Task<User> FindByNameAsync(string name);
}
