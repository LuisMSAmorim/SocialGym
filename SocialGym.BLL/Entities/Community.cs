using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.Entities;

public sealed class Community
{
    [Key]
    public int CommunityId { get; set; }
    [Required]
    [Display(Name = "Nome")]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Descrição")]
    public string Description { get; set; }
}
