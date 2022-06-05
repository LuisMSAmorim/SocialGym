using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.Entities;

public sealed class Community
{
    [Key]
    public int CommunityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
