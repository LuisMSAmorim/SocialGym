
using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.DTOs;

public sealed class UserDTO 
{
    [Required]
    [Display(Name = "Nome de usuário")]
    public string UserName { get; set; }
    [Required]
    [Display(Name = "Senha")]
    public string Password { get; set; }

}
