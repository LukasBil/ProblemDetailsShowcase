using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProblemDetailsShowcase.Exceptions;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProblemDetailsShowcase
{
    public static class CustomExceptionHandler
    {
        private static ProblemDetailsFactory _problemDetailsFactory;
        private static HttpContext _context;
        private static bool _includeStackTrace;

        public static void UseCustomeExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            _includeStackTrace = env.IsDevelopment();
            app.Use(async (context, next) => await WriteResponse(context));
        }

        private static async Task WriteResponse(HttpContext context)
        {
            _context = context;
            var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;

            if (ex != null)
            {
                context.Response.ContentType = "application/problem+json";
                _problemDetailsFactory = context?.RequestServices?.GetRequiredService<ProblemDetailsFactory>();

                ProblemDetails problemDetails = ex switch
                {
                    CustomExceptionBase customException => HandleProblemDetails(customException),
                    ValidationExceptionBase validationException => HandleValidationProblemDetails(validationException),
                    _ => HandleDefault(ex),
                };

                var stream = _context.Response.Body;
                if(problemDetails is ValidationProblemDetails validationProblemDetails)
                {
                    await JsonSerializer.SerializeAsync(stream, validationProblemDetails);
                } else {
                    await JsonSerializer.SerializeAsync(stream, problemDetails);
                }
            }
        }

        private static ProblemDetails HandleDefault(Exception ex)
        {
            var problemDetails = _problemDetailsFactory.CreateProblemDetails(
                _context,
                detail: _includeStackTrace ? ex.ToString() : ex.Message
                );

            return problemDetails;
        }

        private static ProblemDetails HandleProblemDetails(CustomExceptionBase ex)
        {
            _context.Response.StatusCode = (int)ex.StatusCode;
            var problemDetails = _problemDetailsFactory.CreateProblemDetails(
                _context,
                statusCode: (int)ex.StatusCode,
                detail: _includeStackTrace ? ex.ToString() : ex.Message
                );

            return problemDetails;
        }

        private static ValidationProblemDetails HandleValidationProblemDetails(ValidationExceptionBase ex)
        {
            _context.Response.StatusCode = (int)ex.StatusCode;
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError(ex.Property, ex.Message);

            var problemDetails = _problemDetailsFactory.CreateValidationProblemDetails(
                _context,
                statusCode: (int)ex.StatusCode,
                detail: _includeStackTrace ? ex.ToString() : ex.Message,
                modelStateDictionary: modelStateDictionary
                );

            return problemDetails;
        }
    }
}
