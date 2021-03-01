using System;
using System.Net;

namespace ProblemDetailsShowcase.Exceptions
{
    public class CustomValidationException : ValidationExceptionBase
    {
        public CustomValidationException(string property, string message) : base(message)
        {
            Property = property;
            StatusCode = HttpStatusCode.Conflict;
        }

        public CustomValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
