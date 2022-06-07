using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.DTOs;

public sealed class PostDTO
{
    [Required]
    [Display(Name = "Título")]
    public string Title { get; set; }
    [Required]
    [Display(Name = "Texto")]
    public string Text { get; set; }
    [Display(Name = "Imagem")]
    public string ImageUrl { get; set; }
}
