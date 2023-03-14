using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoManager.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> SignInAsync(string email, string password);
        
       Task<string> SignUpAsync(string email, string password);
    }
}
