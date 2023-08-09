using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pyco.Todo.Data.ViewModels;
using System.Text;

namespace Pyco.Todo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async void OnPost([FromForm] string userNameOrEmail, [FromForm] string password)
        {
            string value = await PostLogin(userNameOrEmail, password);
        }

        public async Task<string> PostLogin(string userNameOrEmail, string password)
        {
            AuthenticateRequest authenticateRequest = new()
            {
                Username = userNameOrEmail,
                Password = password
            };

            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7098");

            //var data = new { authenticateRequest };
            string body = System.Text.Json.JsonSerializer.Serialize(authenticateRequest);

            HttpResponseMessage response = await client.PostAsync("/api/v1/Authentication", new StringContent(body, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync();
                return await dataObjects;
            }

            return string.Empty;
        }
    }
}