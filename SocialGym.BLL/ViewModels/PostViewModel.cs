namespace SocialGym.BLL.ViewModels;

public sealed class PostViewModel
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CommunityParticipantId { get; set; }
}
