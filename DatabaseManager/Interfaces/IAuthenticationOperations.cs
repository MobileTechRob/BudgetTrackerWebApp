using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.Interfaces
{
    internal interface IAuthenticationOperations
    {
        bool VerifyUser(string userName, string password);
    }
}
