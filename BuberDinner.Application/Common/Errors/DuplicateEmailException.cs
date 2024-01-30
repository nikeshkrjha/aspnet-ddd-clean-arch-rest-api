using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BuberDinner.Application.Common.Errors
{
    public class DuplicateEmailException : Exception, IServiceException
    {
        public DuplicateEmailException(string message) : base(message)
        {

        }

        public string ErrorMessage => "Email already exists!";
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    }
}