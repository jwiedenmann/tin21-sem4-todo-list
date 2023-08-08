using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Pyco.Todo.Core.Exception;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = error switch
            {
                // 401 means the user needs to supply credentials, 403 means missing permissions
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                // unhandled error
                _ => (int)HttpStatusCode.InternalServerError,
            };
            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}
