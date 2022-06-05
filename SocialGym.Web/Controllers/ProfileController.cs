using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialGym.BLL.ViewModels;
using System.Net.Http.Headers;
using System.Text;

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

        ViewBag.UserName = profile.UserName;
        return View(profile);
    }

    [Route("/profile/edit/{userName}")]
    public async Task<IActionResult> Edit(string userName)
    {
        string loggedUserName = Request.Cookies["username"];

        if(loggedUserName != userName)
        {
            return Unauthorized();
        }

        HttpClient httpClient = new();

        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/profiles/{userName}");

        string apiResponse = await response.Content.ReadAsStringAsync();

        var profile = JsonConvert.DeserializeObject<UserProfileViewModel>(apiResponse);

        if (profile.UserName == null)
        {
            ViewBag.ErrorMessage = "Usuário não encontrado";
            return View(null);
        }

        ViewBag.UserName = profile.UserName;
        return View(profile);
    }

    // POST: BeersController/Edit/
    [HttpPost]
    [Route("/profile/edit/{userName}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(IFormCollection collection)
    {
        string token = Request.Cookies["token"];
        string userName = Request.Cookies["username"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        UserProfileViewModel profile = CreateUserProfileViewModel(userName, collection);

        StringContent content = new(JsonConvert.SerializeObject(profile), Encoding.UTF8, "application/json");

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.PutAsync($"{baseUrl}/profiles/{userName}", content);

        if(userName != profile.UserName && response.IsSuccessStatusCode)
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("username");
            return RedirectToAction("Index", "Login");
        }else if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.ErrorMessage = "Ops... Parece que houve um erro";
        return View(profile);
    }

    private static UserProfileViewModel CreateUserProfileViewModel(string userName, IFormCollection collection)
    {
        return new UserProfileViewModel() {
            Avatar = collection["Avatar"],
            UserName = userName,
            DeadLiftPR = int.Parse(collection["DeadLiftPR"]),
            BackSquatPR = int.Parse(collection["BackSquatPR"]),
            BenchPressPR = int.Parse(collection["BenchPressPR"])
        };
    }
}
