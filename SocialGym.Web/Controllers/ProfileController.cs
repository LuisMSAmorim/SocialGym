using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialGym.BLL.ViewModels;

namespace SocialGym.Web.Controllers;

public class ProfileController : Controller
{

    private readonly string baseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Urls")["ApiUrl"];

    [Route("/profile/{userName}")]
    public async Task<IActionResult> Index(string userName)
    {
        HttpClient httpClient = new();

        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/profiles/{userName}");

        string apiResponse = await response.Content.ReadAsStringAsync();

        var profile = JsonConvert.DeserializeObject<UserProfileViewModel>(apiResponse);

        if(profile.UserName == null)
        {
            ViewBag.ErrorMessage = "Usuário não encontrado";
            return View(null);
        }

        return View(profile);
    }
}
