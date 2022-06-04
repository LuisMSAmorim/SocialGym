using Microsoft.AspNetCore.Mvc;
using SocialGym.BLL.Interfaces;

namespace SocialGym.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommunitiesController : ControllerBase
{
    private readonly ICommunitiesRepository _communitiesRepository;

    public CommunitiesController
    (
        ICommunitiesRepository communitiesRepository
    )
    {
        _communitiesRepository = communitiesRepository;
    }


}
