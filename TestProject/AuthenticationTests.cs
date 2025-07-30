namespace TestProject;
using BudgetAPI.Controllers;
using BudgetAPI.Interfaces;
using Moq;

[TestClass]
public class AuthenticationTests
{
    [TestMethod]
    public void VerifyAuthenticationThrowsNoImplemented()
    {
        var authentication = new Mock<IAuthenticationService>();
        authentication.Setup(x => x.VerifyUser(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(new NotImplementedException()); 

    }
}
