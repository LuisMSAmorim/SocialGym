namespace SocialGym.BLL.Entities;

public sealed class PersonalRecords
{
    public int BenchPressPR { get; set; }
    public int BackSquatPR { get; set; }
    public int DeadLiftPR { get; set; }
    public User User { get; set; }
}
