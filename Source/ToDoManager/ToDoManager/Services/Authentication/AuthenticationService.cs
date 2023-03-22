using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoManager.HelpClasses;
using ToDoManager.Models;

namespace ToDoManager.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private HttpClient _httpClient = new();

        public async Task<string> SignInAsync(string email, string password)
        {
            User user = new()
            {
                Email = email,
                Password = password
            };
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync($"{GlobalVariables._baseAddres}" + "login", JsonContent.Create(user));
            }
            catch
            {
                return "Error";
            }
            return Other.SignInResponse(response);
        }

        public async Task<string> SignUpAsync(string email, string password)
        {      
            User user = new User()
            {
                Email = email,
                Password = password
            };
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync($"{GlobalVariables._baseAddres}" + "users", JsonContent.Create(user));
            }
            catch
            {
                return "Error";
            }
            return Other.SignUpResponse(response);
        }
    }
}
