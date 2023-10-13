using System.Text.Json;

namespace OwnerGPT.WebUI.Admin.Configuration.ExceptionHandlers
{
    public class MiddlewareExceptionHandler
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<MiddlewareExceptionHandler> Logger;

        public MiddlewareExceptionHandler(RequestDelegate next, ILogger<MiddlewareExceptionHandler> logger) =>
            (Next, Logger) = (next, logger);

        public async Task Invoke(HttpContext context)
        {
            try { await Next(context); }
            catch (Exception exception) { await HandleExceptionAsync(context, exception, Logger); }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILogger<MiddlewareExceptionHandler> logger)
        {

            // To help troubleshoot the issue, trace id will be shown to the user
            string traceId = context.TraceIdentifier.ToString();
            int statusCode = 500;

            // Show custom error thrown by the developer or Internal Error Message if unhandled error
            string? exceptionMessage = "Internal Server Error";


            // Object holds the error details that will be logged in the server
            dynamic loggingErrorObject = new
            {
                StatusCode = statusCode,
                exception.Message,
                exception.StackTrace,
                exception.InnerException,
                Cause = "Unhandled exception",
                TraceId = traceId
            };

            // Convert logging error object to string and log it
            string serializedErrorObject = JsonSerializer.Serialize(loggingErrorObject);
            logger.LogError(serializedErrorObject);

            if(context.Request.Path.StartsWithSegments("/api")) {
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    StatusCode = statusCode,
                    ErrorMessage = exceptionMessage,
                    TraceId = traceId
                }));
            } else
            {
                context.Response.Redirect($"/Home/error?message=" + exceptionMessage + "&traceid=" + traceId);
            }
        }
    }
}
