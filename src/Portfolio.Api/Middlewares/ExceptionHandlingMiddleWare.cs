using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace Portfolio.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleWare : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleWare> _logger;

        public ExceptionHandlingMiddleWare(ILogger<ExceptionHandlingMiddleWare> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(ArgumentException ex)
            {
                _logger.LogWarning(ex, "Bad request: {Message}", ex.Message);

                await WriteProblemDetailsAsync(
                    context,
                    statusCode: (int)HttpStatusCode.BadRequest,
                    title: "Invalid request",
                    detail: ex.Message
                    );
            }
            
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                await WriteProblemDetailsAsync(
                    context,
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    title: "Server error",
                    detail: "An unexpected error occured."
                    );
            }
        }

        private static async Task WriteProblemDetailsAsync(HttpContext context, int statusCode, string title,string detail)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem.json";

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
