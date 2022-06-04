using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.Entities;

public sealed class CommunityParticipant
{
    [Key]
    public int CommunityParticipantId { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    public int CommunityId { get; set; }
    public Community Community { get; set; }
    [Required]
    public bool IsAdmin { get; set; }
}
