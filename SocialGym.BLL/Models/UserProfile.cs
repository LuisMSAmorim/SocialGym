using SocialGym.BLL.Entities;

namespace SocialGym.BLL.Models;

public class UserProfile
{
    public string UserName { get; set; }
    public string Avatar { get; set; }
    public int BenchPressPR { get; set; }
    public int BackSquatPR { get; set; }
    public int DeadLiftPR { get; set; }
    public IEnumerable<Community> Communities { get; set; }
}
