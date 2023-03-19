using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ToDoManager.HelpClasses;
using ToDoManager.Models;

namespace ToDoManager.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService

    {
        private HttpClient _httpClient = new();


        public async Task<string> SignInAsync(string email, string password)
        {           
            User user = new User()
            {
                Email = email,
                Password = password
            };
            //shiet code replace 
            var uriBuilder = new UriBuilder($"{GlobalVariables._baseAddres}" + "login");
            var paramValues = HttpUtility.ParseQueryString(uriBuilder.Query);
            paramValues.Add("email", user.Email);
            paramValues.Add("password", user.Password);
            paramValues.Add("device", InfoDevice.DeviceModel);
            uriBuilder.Query = paramValues.ToString();
            HttpResponseMessage response;   
            try
            {
                response = await _httpClient.GetAsync(uriBuilder.ToString());
            }
            catch
            {
                return "Error";
            }

            return Other.SignInResponse(response);

        }
        
        public async Task<string> SignUpAsync(string email, string password)
        {
            // http cringe intilization in method         
            User user = new User()
            {
                Email = email,
                Password = password
            };    
            StringContent content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync($"{GlobalVariables._baseAddres}"+"users", content);
            }
            catch
            {
                return "Error";
            }

            return Other.SignUpResponse(response);
        }
    }
}
