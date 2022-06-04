using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;
using SocialGym.BLL.Models;
using SocialGym.DAL.Context;

namespace SocialGym.DAL.Repositories;

public sealed class UsersRepository : IUsersRepository
{
    private readonly SocialGymDbContext _context;
    private readonly UserManager<User> _userManager;

    public UsersRepository
    (
        SocialGymDbContext context,
        UserManager<User> userManager
    )
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IdentityResult> AddAsync(UserDetails userDetails)
    {
        User user = new() { UserName = userDetails.UserName, Email = userDetails.Email };
        return await _userManager.CreateAsync(user, userDetails.Password);
    }

    public async Task DeleteAsync(User user)
    {
        await _userManager.DeleteAsync(user);
    }

    public async Task<User> FindByNameAsync(string name)
    {
        return await _userManager.FindByNameAsync(name);
    }

    public async Task<User> GetByIdAsync(string id)
    {
        return await _context.User.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateProfileAsync(string id, UserProfile profile)
    {
        var user = await _context.User.SingleOrDefaultAsync(x => x.Id == id);

        _context.Entry(user).CurrentValues.SetValues(profile);

        await _context.SaveChangesAsync();
    }
}
