using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next1;

    private readonly ILogger<ExceptionMiddleware> logger1;

    private readonly IHostEnvironment env1;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        next1 = next;
        logger1 = logger;
        env1 = env;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next1(context);
        }

        catch (Exception ex)
        {
            logger1.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env1.IsDevelopment()
            ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
            : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }


}
