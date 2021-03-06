namespace SocialGym.BLL.ViewModels;

public sealed class LoginResponseViewModel
{
    #nullable enable
    public string? Token { get; set; }
    #nullable enable
    public string? UserName { get; set; }
    #nullable disable
    public string Message { get; set; }
}
