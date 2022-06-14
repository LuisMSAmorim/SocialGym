using System.ComponentModel.DataAnnotations;

namespace SocialGym.BLL.ViewModels;

public sealed class UserProfileViewModel
{
    [Display(Name = "Nome de usuário")]
    public string UserName { get; set; }
    public string Avatar { get; set; }
    [Display(Name = "Recorde pessoal - Supino")]
    public int BenchPressPR { get; set; }
    [Display(Name = "Recorde pessoal - Agachamento")]
    public int BackSquatPR { get; set; }
    [Display(Name = "Recorde pessoal - Levantamento Terra")]
    public int DeadLiftPR { get; set; }
}
