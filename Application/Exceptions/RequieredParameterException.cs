using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    [Serializable]
    public class RequiredParameterException : Exception
    {
        public RequiredParameterException()
        {
        }

        public RequiredParameterException(string? message)
            : base(message) { }

        public RequiredParameterException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
