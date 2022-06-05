using Microsoft.AspNetCore.Identity;
using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;
using SocialGym.BLL.ViewModels;

namespace SocialGym.BLL.Interfaces;

public interface IUsersRepository
{
    Task<IdentityResult> AddAsync(UserDTO userDetails);
    Task DeleteAsync(User user);
    Task<IdentityResult> UpdateProfileAsync(string id, UserProfileViewModel profile);
    Task<User> GetByIdAsync(string id);
    Task<User> GetByNameAsync(string name);
}
