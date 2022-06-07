using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.Entities;

public sealed class User : IdentityUser
{
    public string Avatar { get; set; }
    [Display(Name = "Recorde pessoal - Supino")]
    public int BenchPressPR { get; set; }
    [Display(Name = "Recorde pessoal - Agachamento")]
    public int BackSquatPR { get; set; }
    [Display(Name = "Recorde pessoal - Levantamento Terra")]
    public int DeadLiftPR { get; set; }
}
