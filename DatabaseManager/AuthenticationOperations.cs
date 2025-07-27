using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatabaseManager.DataModels;
using SharedDataModels;
using Microsoft.EntityFrameworkCore;
using DatabaseManager.Interfaces;


namespace DatabaseManager
{
    public class AuthenticationOperations  : IAuthenticationOperations
    {
        AppDbContext appDbContext;
        ILogger logger;

        public AuthenticationOperations(AppDbContext appDbContext, ILogger logger)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
        }

        public bool VerifyUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                logger.LogWarning("Username or password is null or empty.");
                return false;
            }
            // Implement user verification logic here
            // For now, we will return true to indicate that the user is verified
            return true;
        }
    }
}
