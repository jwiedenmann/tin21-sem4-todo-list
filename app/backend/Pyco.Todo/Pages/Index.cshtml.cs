﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using Pyco.Todo.Data.ViewModels;
using System;
using System.Text;
using System.Text.Json;

namespace Pyco.Todo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string FormResult { get; set; } = string.Empty;
        public void OnGet() { }

        /// <summary>
        /// OnPost is called if the user hits the submit button
        /// </summary>
        /// <param name="userNameOrEmail">Username or useremail</param>
        /// <param name="password">Password of the user</param>
        /// <returns>Returns actionresult</returns>
        public async Task<IActionResult> OnPost([FromForm] string userNameOrEmail, [FromForm] string password)
        {
            string baseAddress = _configuration.GetValue<string>("TodoApi:BaseAddress");
            string authentication = _configuration.GetValue<string>("TodoApi:Authentication");
            string todo = _configuration.GetValue<string>("TodoApi:Todo");
            string address = Flurl.Url.Combine(baseAddress, authentication);

            AuthenticateRequest authenticateRequest = new()
            {
                Username = userNameOrEmail,
                Password = password
            };

            using HttpClient client = new();
            HttpRequestMessage message = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(address),
                Content = new StringContent(JsonSerializer.Serialize(authenticateRequest), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                //TODO do something in view
                FormResult =
                    $"The username or password you eneterd is incorrect. Please try again.";
                return Page();
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            AuthenticateResponse? authenticateResponse = JsonSerializer
                .Deserialize<AuthenticateResponse>(
                    responseContent,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            string? refreshToken = response.Headers
                .SingleOrDefault(header => header.Key == "Set-Cookie")
                .Value
                .FirstOrDefault(x => x.StartsWith("refreshToken"));

            if (authenticateResponse is null ||
                !SetCookieHeaderValue.TryParse(refreshToken, out SetCookieHeaderValue? refreshCookie) ||
                refreshCookie is null)
            {
                return BadRequest();
            }

            SetCookieHeaderValue jwtCookie = new("jwt", authenticateResponse.JwtToken);
            AppendCookie(Response, jwtCookie);
            AppendCookie(Response, refreshCookie);
            return Redirect(todo);
        }

        private void AppendCookie(HttpResponse response, SetCookieHeaderValue setCookie)
        {
            CookieOptions cookieOptions = new()
            {
                Expires = setCookie.Expires,
                HttpOnly = setCookie.HttpOnly,
                MaxAge = setCookie.MaxAge
            };

            response.Cookies.Append(setCookie.Name.ToString(), setCookie.Value.ToString(), cookieOptions);
        }
    }
}