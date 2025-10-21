using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Text.Json;


namespace GlobalConstants
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {


            var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

            (int StatusCode, string Title, string Type) = exception switch
            {
                InvalidCastException invalidCastException => (400, invalidCastException.Message, ""),
                AggregateException aggregateException => (400, aggregateException.Message, ""),
                ArgumentNullException argumentNullException => (400, argumentNullException.Message, ""),
                ArgumentException argumentException => (500, argumentException.Message, ""),
                DuplicateException duplicateException => (409, duplicateException.Message, duplicateException.Type),
                // ValidationException validationException => (400, validationException.Message),
                KeyNotFoundException keyNotFoundException => (404, keyNotFoundException.Message, ""),
                FormatException formatException => (400, formatException.Message, ""),
                MySqlException mySqlException => (400, mySqlException.Message, ""),
                //ForbidException => (403, "Forbidden"),
                BadHttpRequestException => (400, "Bad request", ""),
                NotImplementedException notImplementedException => (500, notImplementedException.Message, ""),
                NotFoundException notfnotfound => (404, notfnotfound.Message, notfnotfound.Type),
                UnauthorizedAccessException unauthorizedAccessException => (401, unauthorizedAccessException.Message, ""),
                InvalidOperationException invalidOperationException => (500, invalidOperationException.Message, ""),
                _ => (500, "An error occured @" + exception.Message, "")
            };



            var problemDetails = new ProblemDetails
            {

                Detail = exception.InnerException?.Message ?? exception.Message,
                Type = Type,
                Title = Title,
                Status = StatusCode,
                Instance = exceptionHandlerFeature?.Endpoint?.ToString() ?? $"{httpContext.Request.Method} {httpContext.Request.Path}",

            };
            // _logger.LogError("Exception and problem details {0},{1}", JsonConvert.SerializeObject(problemDetails), exception);
            _logger.LogError("Exception and problem details {0},{1}", JsonSerializer.Serialize(problemDetails), exception);

            //  _logger.LogError("Exception and problem details {0},{1}", JsonSerializer.Serialize<ProblemDetails(problemDetails), exception);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;

        }


    }
    public class NotFoundException : Exception
    {
        public string Type = "A07";
        public string Title = "Data Not Found  in Repository";
        public int statusCode = 404;
        public NotFoundException(string name, object key)
            : base("Data Not Found  in Repository")
        {
        }
    }

    public class DuplicateException : Exception
    {
        public string Type = "A01";
        public string Title = "Data  already Exist in Repository";
        public int statusCode = 409;
        public DuplicateException(string name, object key)
            : base("Data  already Exist in Repository")
        {
        }
    }
}
