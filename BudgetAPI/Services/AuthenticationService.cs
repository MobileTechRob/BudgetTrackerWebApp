
using BudgetAPI.Interfaces;

namespace BudgetAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService()
        {
            // Initialization code if needed
        }

        bool IAuthenticationService.VerifyUser(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
