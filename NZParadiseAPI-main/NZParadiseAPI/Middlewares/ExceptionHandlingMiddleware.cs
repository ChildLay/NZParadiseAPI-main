// Middlewares/ExceptionHandlingMiddleware.cs
using Microsoft.AspNetCore.Http;
using NZParadiseAPI.Library;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Continue the request pipeline
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log the error here if you want (e.g., Serilog, Application Insights)

        var response = new ResponseObject
        {
            Status = 0,
            Message = "An unexpected error occurred. Please try again."
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(result);
    }
}
