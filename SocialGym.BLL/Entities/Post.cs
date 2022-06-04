using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.Entities;

public sealed class Post
{
    [Key]
    public int PostId { get; set; }
    [Required]
    public string Title { get; set; }
    public string Text { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    [Required]
    public int CommunityParticipantId { get; set; }

    [Required]
    public CommunityParticipant CommunityParticipant { get; set; }
}
