using Application.Exceptions;
using Application.Exceptions.Application.Exceptions;
using Application.Models.Response;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Restaurante_Proyecto.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
       
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
               
                _logger.LogError(ex, $"Catch Error: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            string message = ex.Message;

            switch (ex)
            {
                case RequiredParameterException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case InvalidateParameterException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                    statusCode = HttpStatusCode.BadRequest;
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case ConflictException:
                    statusCode = HttpStatusCode.Conflict;
                    break;
                case OrderPriceException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = ex.Message;
                    break;


            }
            context.Response.StatusCode = (int)statusCode;

            var error = new ApiError(message);
            var result = JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(result);
        }
    }
}

