using SocialGym.BLL.Entities;
using SocialGym.BLL.ViewModels;

namespace SocialGym.Web.Models;

public sealed class UserProfileCommunitiesViewModel
{
    public UserProfileViewModel UserProfile { get; set; }
    public IEnumerable<Community> Communities { get; set; }    
}
