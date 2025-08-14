
using BudgetAPI.Interfaces;
using DatabaseManager.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticationOperations authenticationOperations;
        string jwtKey = "";

        public AuthenticationService(IAuthenticationOperations authenticationOperations, IConfiguration configuration)
        {
            // Initialization code if needed
            this.authenticationOperations = authenticationOperations;           
            jwtKey = configuration.GetSection("AuthenticationSettings").GetSection("JwtKey").Value!;
        }

        public string GenerateJwtToken(string userName)
        {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {                    
                    new Claim(ClaimTypes.Name, userName, ClaimValueTypes.String),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var token = new JwtSecurityToken(
                    issuer: "MyApp",
                    audience: "MyAppUsers",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
        }

        bool IAuthenticationService.VerifyUser(string userName, string password)
        {
            return authenticationOperations.VerifyUser(userName, password);            
        }
    }
}
