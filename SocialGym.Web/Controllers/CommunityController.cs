using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;
using SocialGym.BLL.ViewModels;
using System.Net.Http.Headers;
using System.Text;

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

    public IActionResult Create()
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IFormCollection collection)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        CommunityDTO community = CreateCommunityDTOWithFormProps(collection);

        StringContent content = new(JsonConvert.SerializeObject(community), Encoding.UTF8, "application/json");

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.PostAsync($"{baseUrl}/communities/", content);

        string apiResponse = await response.Content.ReadAsStringAsync();

        JsonConvert.DeserializeObject<CreatedAtActionResult>(apiResponse);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Participants(int id)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/communities/participants/{id}");

        string apiResponse = await response.Content.ReadAsStringAsync();

        var communityParticipants = JsonConvert.DeserializeObject<List<UserProfileViewModel>>(apiResponse);

        if (communityParticipants == null || communityParticipants.Count == 0)
        {
            ViewBag.ErrorMessage = "Participantes não encontrados";
            return View();
        }

        return View(communityParticipants);
    }

    public async Task<IActionResult> Ingress(int id)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/communities/{id}");

        string apiResponse = await response.Content.ReadAsStringAsync();

        var community = JsonConvert.DeserializeObject<Community>(apiResponse);

        if (community == null)
        {
            ViewBag.ErrorMessage = "Comunidade não encontrada";
            return View();
        }

        return View(community);
    }

    [HttpPost]
    public async Task<IActionResult> Ingress(IFormCollection collection)
    {
        string token = Request.Cookies["token"];

        int communityId = int.Parse(collection["CommunityId"]);

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await httpClient.PostAsync($"{baseUrl}/communities/become_participant/{communityId}", null);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Admin(int id)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/communities/admin/{id}");

        string apiResponse = await response.Content.ReadAsStringAsync();

        var admin = JsonConvert.DeserializeObject<UserProfileViewModel>(apiResponse);

        if (admin == null )
        {
            ViewBag.ErrorMessage = "Admin não encontrado";
            return View();
        }

        return View(admin);
    }

    public async Task<IActionResult> Posts(int id)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/posts/communities/{id}");

        string apiResponse = await response.Content.ReadAsStringAsync();

        var posts = JsonConvert.DeserializeObject<List<PostViewModel>>(apiResponse);

        if (posts == null || posts.Count == 0)
        {
            ViewBag.ErrorMessage = "Posts não encontrados";
            return View();
        }

        return View(posts);
    }

    private static CommunityDTO CreateCommunityDTOWithFormProps(IFormCollection collection)
    {
        return new CommunityDTO()
        {
            Description = collection["Description"],
            Name = collection["Name"],
        };
    }
}
