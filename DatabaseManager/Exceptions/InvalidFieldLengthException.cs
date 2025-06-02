using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.Exceptions
{
    public class InvalidFieldLengthException : Exception
    {
        public InvalidFieldLengthException(string message) : base(message)
        {
        }
        public InvalidFieldLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public InvalidFieldLengthException() : base("The length of a field exceeds the allowed limit in the database.")
        {
        }
    }
}
