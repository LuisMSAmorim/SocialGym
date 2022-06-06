using SocialGym.BLL.Entities;
using SocialGym.BLL.ViewModels;

namespace SocialGym.Web.Models;

public class CommunityPostsViewModel
{
    public List<PostViewModel> Posts { get; set; }
    public Community Community { get; set; }
}
