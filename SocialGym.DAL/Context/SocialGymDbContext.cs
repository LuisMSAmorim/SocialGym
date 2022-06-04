using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialGym.BLL.Entities;

namespace SocialGym.DAL.Context;

public class SocialGymDbContext : IdentityDbContext<IdentityUser>
{
    public SocialGymDbContext(DbContextOptions<SocialGymDbContext> options) : base(options) { }

    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<Community> Community { get; set; }
    public virtual DbSet<CommunityParticipant> CommunityParticipant { get; set; }
    public virtual DbSet<Post> Post { get; set; }
}
