using HeroApp.App.Common.Exceptions;
using HeroApp.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace HeroApp.Api.Common
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() {  PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case DbUpdateException dbUpdateException:
                    code = HttpStatusCode.Conflict;
#if !DEBUG
                    logger.LogError("Erro conflito no banco de dados");
                    logger.LogError("Erro conflito no banco de dados", dbUpdateException);
                    result = JsonSerializer.Serialize(ApiResponse.Failure("Erro conflito no banco de dados"), options: jsonOptions);
#endif
#if DEBUG
                    result = JsonSerializer.Serialize(ApiResponse.Failure(dbUpdateException.Message), options: jsonOptions);
#endif

                    break;
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;                   
                    result = JsonSerializer.Serialize(ApiResponse.Failure(validationException.Failures), options: jsonOptions);
                    break;

                case DbRollBackException dbRollBackException:
                    code = HttpStatusCode.NotImplemented;
                      result = JsonSerializer.Serialize(ApiResponse.Failure(dbRollBackException.ListStatckTrace), options: jsonOptions);
                    break;
                 default:
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonSerializer.Serialize(ApiResponse.Failure(exception.Message), options: jsonOptions);
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
