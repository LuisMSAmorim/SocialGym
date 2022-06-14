using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;
using SocialGym.BLL.ViewModels;
using SocialGym.Web.Models;
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

        var response = await httpClient.PostAsync($"{baseUrl}/communities/become_participant/{communityId}", null);

        if (response.IsSuccessStatusCode == false)
        {
            ViewBag.ErrorMessage = "Ops, parece que você já é participante desta comunidade";
            return View();
        }

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
    
    [HttpGet]
    [Route("/Community/user/{userName}")]
    public async Task<IActionResult> UserCommunities(string userName)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Task.WhenAll(
            httpClient.GetAsync($"{baseUrl}/communities/user/{userName}"),
            httpClient.GetAsync($"{baseUrl}/profiles/{userName}"));

        var communitiesResponse = response[0];
        var userProfileResponse = response[1];

        if(userProfileResponse.IsSuccessStatusCode == false)
        {
            ViewBag.ErrorMessage = "Usuário não encontrado";
            return View();
        }

        var apiResponse = await Task.WhenAll(
            communitiesResponse.Content.ReadAsStringAsync(),
            userProfileResponse.Content.ReadAsStringAsync());

        string communitiesApiResponse = apiResponse[0];
        string userProfileApiResponse = apiResponse[1];

        var userCommunities = JsonConvert.DeserializeObject<List<Community>>(communitiesApiResponse);
        var userProfile = JsonConvert.DeserializeObject<UserProfileViewModel>(userProfileApiResponse);

        if (userCommunities == null || userCommunities.Count == 0)
        {
            ViewBag.ErrorMessage = "Comunidades não encontradas";
            return View();
        }

        UserProfileCommunitiesViewModel userProfileCommunities = new()
        {
            Communities = userCommunities,
            UserProfile = userProfile
        };

        return View(userProfileCommunities);
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
