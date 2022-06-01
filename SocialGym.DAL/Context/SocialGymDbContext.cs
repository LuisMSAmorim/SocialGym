using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SocialGym.DAL.Context;

public class SocialGymDbContext : IdentityDbContext<IdentityUser>
{
    public SocialGymDbContext(DbContextOptions<SocialGymDbContext> options) : base(options) { }
}
