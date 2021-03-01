using System;
using System.Net;

namespace ProblemDetailsShowcase.Exceptions
{
    public class NotFoundException : CustomExceptionBase
    {
        public NotFoundException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.NotFound;
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
