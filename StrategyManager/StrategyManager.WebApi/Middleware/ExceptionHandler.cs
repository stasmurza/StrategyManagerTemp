using StrategyManager.Core.Exceptions;
using StrategyManager.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace StrategyManager.WebAPI.Middleware
{
    /// <summary>
    /// Exception handler middleware
    /// </summary>
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public ExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<ExceptionHandler>();
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            logger.LogError(ex, ex.Message);

            HttpStatusCode code = HttpStatusCode.InternalServerError;
            int errorCode = ErrorStatusCodes.ServerInternalError;
            var errorMessage = "Internal server error";

            if (ex is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
                errorCode = ((NotFoundException)ex).ErrorCode;
                errorMessage = ex.Message;
            }
            else if (ex is ErrorCodeException)
            {
                code = HttpStatusCode.BadRequest;
                errorCode = ((ErrorCodeException)ex).ErrorCode;
                errorMessage = ex.Message;
            }
            else if (ex is ValidationException)
            {
                code = HttpStatusCode.BadRequest;
                errorCode = ErrorStatusCodes.BadRequest;
                errorMessage = ex.Message;
            }
            else if (ex is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Forbidden;
                errorCode = ErrorStatusCodes.Unathorized;
                errorMessage = "You are not authorized";
            }

            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";

            object response = new ErrorResponse
            {
                Code = errorCode.ToString(),
                Description = errorMessage,
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var serliazedResponse = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(serliazedResponse);
        }
    }
}
