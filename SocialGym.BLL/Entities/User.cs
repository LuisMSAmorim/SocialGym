﻿using Microsoft.AspNetCore.Identity;

namespace SocialGym.BLL.Entities;

public sealed class User : IdentityUser
{
    public string Avatar { get; set; }
    public int BenchPressPR { get; set; }
    public int BackSquatPR { get; set; }
    public int DeadLiftPR { get; set; }
}
