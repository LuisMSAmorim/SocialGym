using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialGym.BLL.DTOs;
using SocialGym.BLL.Entities;
using SocialGym.BLL.ViewModels;
using SocialGym.Web.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SocialGym.Web.Controllers;

public class PostController : Controller
{
    private readonly string baseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Urls")["ApiUrl"];

    // GET: post/communityId
    [Route("/posts/{communityId}")]
    public async Task<IActionResult> Index(int communityId)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Task.WhenAll(
            httpClient.GetAsync($"{baseUrl}/communities/{communityId}"),
            httpClient.GetAsync($"{baseUrl}/posts/communities/{communityId}")
        );

        var communityResponse = response[0];
        var postsResponse = response[1];

        if (postsResponse.IsSuccessStatusCode == false)
        {
            ViewBag.NonParticipantMessage = "Oops, algo deu errado... Ingresse nesta comunidade e tente novamente!";
            return View();
        }

        string communityApiResponse = await communityResponse.Content.ReadAsStringAsync();
        string postsApiResponse = await postsResponse.Content.ReadAsStringAsync();

        var community = JsonConvert.DeserializeObject<Community>(communityApiResponse);
        var posts = JsonConvert.DeserializeObject<List<PostViewModel>>(postsApiResponse);

        CommunityPostsViewModel communityPosts = new()
        {
            Community = community,
            Posts = posts
        };

        if (community == null)
        {
            ViewBag.ErrorMessage = "Comunidade não encontrada";
            return View();
        }
        if (posts == null || posts.Count == 0)
        {
            ViewBag.ErrorMessage = "Posts não encontrados";
            return View(communityPosts);
        }

        return View(communityPosts);
    }

    [Route("/post/create/{communityId}")]
    public IActionResult Create(int communityId)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        ViewBag.CommunityId = communityId;
        return View();
    }

    [HttpPost]
    [Route("/post/create/{communityId}")]
    public async Task<IActionResult> Create(int communityId, IFormCollection collection)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        PostDTO post = CreatePostDTOWithFormProps(collection);

        StringContent content = new(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.PostAsync($"{baseUrl}/posts/{communityId}", content);

        string apiResponse = await response.Content.ReadAsStringAsync();

        JsonConvert.DeserializeObject<CreatedAtActionResult>(apiResponse);

        ViewBag.CommunityId = communityId;
        ViewBag.SuccessMessage = "Post criado com sucesso";
        return View();
    }

    [HttpGet]
    [Route("/post/delete/{postId}")]
    public async Task<IActionResult> Delete(int postId)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/posts/{postId}");

        if (response.IsSuccessStatusCode == false)
        {
            ViewBag.NonParticipantMessage = "Oops, algo deu errado... Ingresse nesta comunidade e tente novamente!";
            return View();
        }

        string apiResponse = await response.Content.ReadAsStringAsync();

        var post = JsonConvert.DeserializeObject<PostViewModel>(apiResponse);

        if (post == null)
        {
            ViewBag.ErrorMessage = "Post não encontrado";
            return View();
        }

        return View();
    }

    [HttpPost]
    [Route("/post/delete/{postId}")]
    public async Task<IActionResult> Delete(int postId, IFormCollection collection)
    {
        string token = Request.Cookies["token"];

        if (token == null)
        {
            return RedirectToAction("Index", "Login");
        }

        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.DeleteAsync($"{baseUrl}/posts/{postId}");

        if (response.IsSuccessStatusCode)
        {
            ViewBag.SuccessMessage = "Post deletado com sucesso";
            return View();
        }

        PostDTO post = CreatePostDTOWithFormProps(collection);

        ViewBag.ErrorMessage = "Oops, parece que você não é autor do post ou administrador da comunidade...";
        return View(post);
    }

    private static PostDTO CreatePostDTOWithFormProps(IFormCollection colleciton)
    {
        return new PostDTO()
        {
            Title = colleciton["Title"],
            Text = colleciton["Text"],
            ImageUrl = colleciton["ImageUrl"]
        };
    }
}
