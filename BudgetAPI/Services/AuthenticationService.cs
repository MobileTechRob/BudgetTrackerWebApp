
using BudgetAPI.Interfaces;
using DatabaseManager.Interfaces;

namespace BudgetAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticationOperations authenticationOperations;

        public AuthenticationService(IAuthenticationOperations authenticationOperations)
        {
            // Initialization code if needed
            this.authenticationOperations = authenticationOperations;
        }

        bool IAuthenticationService.VerifyUser(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
