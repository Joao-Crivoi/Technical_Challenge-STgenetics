using System.Net;
using System.Text.Json;
using GoodHamburger.Api.Domain.Exceptions;

namespace GoodHamburger.Api.Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            Console.WriteLine($"\n!!! ERRO DETECTADO: {ex.Message}");
            if (ex.InnerException != null) 
                Console.WriteLine($"!!! INNER EXCEPTION: {ex.InnerException.Message}");
            
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = exception switch
        {
            DomainException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            message = exception.Message,
            innerError = exception.InnerException?.Message, 
            detail = exception is DomainException ? null : "An internal error occurred."
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled Exception");

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}