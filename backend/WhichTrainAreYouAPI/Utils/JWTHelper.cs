using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WhichTrainAreYouAPI.Models;

namespace WhichTrainAreYouAPI.Utils
{
    public class JWTHelper
    {
        private readonly IConfiguration _configuration;

        public JWTHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJWTToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var appSettings = _configuration.GetSection("AppSettings");
            var privateKey = Environment.GetEnvironmentVariable("WhichTrainAreYouJWTKey");
            //var privateKey = appSettings["JwtSecretKey"];
            var audience = appSettings["Audience"];
            var issuer = appSettings["Issuer"];

            var key = Encoding.ASCII.GetBytes(privateKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = audience,
                Issuer = issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}