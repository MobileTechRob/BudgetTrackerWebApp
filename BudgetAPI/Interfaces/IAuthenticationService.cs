namespace BudgetAPI.Interfaces
{
    public interface IAuthenticationService
    {
        bool VerifyUser(string userName, string password);

        string GenerateJwtToken(string userName);
    }
}
