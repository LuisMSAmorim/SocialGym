using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialGym.BLL.ViewModels;
using System.Text;

namespace SocialGym.Web.Controllers;

public class RegistrationController : Controller
{
    private readonly string baseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Urls")["ApiUrl"];

    // GET: Registration/Index
    public IActionResult Index()
    {
        return View();
    }

    // POST: Registration/Index
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginCredentialsViewModel userDetails)
    {
        HttpClient httpClient = new();

        StringContent content = new(JsonConvert.SerializeObject(userDetails), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync($"{baseUrl}/accounts", content);

        string apiResponse = await response.Content.ReadAsStringAsync();

        var deserializedResponse = JsonConvert.DeserializeObject<RegistrationResponseViewModel>(apiResponse);

        if (deserializedResponse.Errors != null)
        {
            ViewBag.Message = "Oops, parece que sua tentativa de registro falhou... \r\n" +
                "\n Verifique se sua senha contém ao menos uma letra maiúscula, um número e um caracter especial." +
                "\n Caso sua senha esteja dentro do padrão requerido, tente novamente com um nome de usuário diferente...";
            return View();
        }

        TempData["registration"] = $"Usuário {userDetails.Username} registrado com sucesso";
        return RedirectToAction("Index", "Login");
    }
}
