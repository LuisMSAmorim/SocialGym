using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.Entities;

public sealed class Post
{
    [Key]
    public int PostId { get; set; }
    [Required]
    [Display(Name = "Título")]
    public string Title { get; set; }
    [Required]
    [Display(Name = "Texto")]
    public string Text { get; set; }
    [Display(Name = "Imagem")]
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    [Required]
    public int CommunityParticipantId { get; set; }

    [Required]
    public CommunityParticipant CommunityParticipant { get; set; }
}
