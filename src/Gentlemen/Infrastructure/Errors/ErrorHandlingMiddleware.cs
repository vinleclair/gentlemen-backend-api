using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gentlemen.Infrastructure.Errors
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILogger<ErrorHandlingMiddleware> logger
            )
        {
            string result = null;
            switch (exception)
            {
                case RestException re:
                    context.Response.StatusCode = (int)re.Code;
                    result = JsonConvert.SerializeObject(new
                    {
                        errors = re.Errors
                    });
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    logger.LogError(exception, "Unhandled Exception");
                     result = JsonConvert.SerializeObject(new
                    {
                        errors = Constants.InternalServerError
                    });
                    break;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result ?? "{}");
        }
    }
}