using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDoManager.Asp.Net.Core.Models;

namespace ToDoManager.Asp.Net.Core.Services
{
    public class JwtService
    {
        //private readonly JwtSecurityToken _jwtToken;
        public string issuer;
        public string audience;
        public string secret;
        public int tokenLifeTime;
        public SymmetricSecurityKey symmetricSecurityKey;
        public JwtService(IOptions<JwtOption> jwtOption)
        {

            issuer = jwtOption.Value.Issuer;
            audience = jwtOption.Value.Audience;
            secret = jwtOption.Value.Secret;
            tokenLifeTime = jwtOption.Value.TokenLifeTime;
            symmetricSecurityKey = jwtOption.Value.GetSymmetricSecurityKey();
        }

        public string GenerateToken(string email)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
            };
           var _jwtToken = new JwtSecurityToken(
         issuer,
         audience,
         claims,
         null,
         DateTime.Now.AddMinutes(tokenLifeTime),
         new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
     );
            return new JwtSecurityTokenHandler().WriteToken(_jwtToken);
        }

        public string DecodeToken(string token)
        {
            var decode = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var a = decode.Claims.ToList();
            string email = Convert.ToString(a[0]);
           email= email.Replace("email: ", "");
            return email;
        }

    }
    
}
