using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialGym.BLL.Entities;
using System.Net.Http.Headers;

namespace SocialGym.Web.Controllers;

public class CommunityController : Controller
{
    private readonly string baseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Urls")["ApiUrl"];

    public async Task<IActionResult> Index()
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/communities");

        string apiResponse = await response.Content.ReadAsStringAsync();

        var communities = JsonConvert.DeserializeObject<List<Community>>(apiResponse);

        if (communities == null || communities.Count == 0)
        {
            ViewBag.ErrorMessage = "Comunidades não encontradas";
            return View();
        }

        return View(communities);
    }
}
