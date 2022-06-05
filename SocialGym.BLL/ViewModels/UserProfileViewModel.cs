namespace SocialGym.BLL.ViewModels;

public sealed class UserProfileViewModel
{
    public string UserName { get; set; }
    public string Avatar { get; set; }
    public int BenchPressPR { get; set; }
    public int BackSquatPR { get; set; }
    public int DeadLiftPR { get; set; }
}
