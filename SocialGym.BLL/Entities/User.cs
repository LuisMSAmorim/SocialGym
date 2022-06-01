using Microsoft.AspNetCore.Identity;

namespace SocialGym.BLL.Entities;

public sealed class User : IdentityUser
{
    public string NickName { get; set; }
    public string Avatar { get; set; }
    public PersonalRecords PersonalRecords { get; set; }
}
