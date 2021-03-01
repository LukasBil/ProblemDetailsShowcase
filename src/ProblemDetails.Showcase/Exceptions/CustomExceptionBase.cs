using System;
using System.Net;
using System.Runtime.Serialization;

namespace ProblemDetailsShowcase
{
    public class CustomExceptionBase : Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public CustomExceptionBase(string message) : base(message)
        {
        }

        public CustomExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
