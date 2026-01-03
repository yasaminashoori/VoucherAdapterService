using System.Net;
using System.Text.Json;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ValidationException validationEx => new ApiResponse<object>
            {
                Success = false,
                Message = validationEx.Message,
                ErrorCode = validationEx.ErrorCode
            },
            BusinessException businessEx => new ApiResponse<object>
            {
                Success = false,
                Message = businessEx.Message,
                ErrorCode = businessEx.ErrorCode
            },
            _ => new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while processing your request.",
                ErrorCode = "INTERNAL_SERVER_ERROR"
            }
        };

        var statusCode = exception switch
        {
            ValidationException => 400,
            BusinessException businessEx => businessEx.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = statusCode;

        if (statusCode >= 500)
        {
            _logger.LogError(exception, "An unhandled exception occurred");
        }
        else
        {
            _logger.LogWarning(exception, "Business exception occurred: {Message}", exception.Message);
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(jsonResponse);
    }
}

