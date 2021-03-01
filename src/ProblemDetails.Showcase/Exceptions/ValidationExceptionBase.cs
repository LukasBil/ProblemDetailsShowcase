using System;
using System.Net;

namespace ProblemDetailsShowcase.Exceptions
{
    public class ValidationExceptionBase : Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }
        public string Property { get; protected set; }

        protected ValidationExceptionBase(string message) : base(message)
        {
        }

        protected ValidationExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}