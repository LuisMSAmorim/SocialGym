namespace SocialGym.BLL.Entities;

public sealed class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; }
}
