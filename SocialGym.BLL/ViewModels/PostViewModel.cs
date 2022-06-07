using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.ViewModels;

public sealed class PostViewModel
{
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
    public string CommunityParticipantUserName { get; set; }
    public string CommunityParticipantAvatar { get; set; }
    public int CommunityId { get; set; }
}
