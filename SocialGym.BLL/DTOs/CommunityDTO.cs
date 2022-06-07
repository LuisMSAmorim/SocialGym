using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.DTOs;

public sealed class CommunityDTO
{
    [Required]
    [Display(Name = "Nome")]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Descrição")]
    public string Description { get; set; }
}
