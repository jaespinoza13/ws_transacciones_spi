using Application.Common.Models;
using Microsoft.Extensions.Options;
using WebApi.Middleware.Common;

namespace WebApi.Middleware;

public class Authorization(RequestDelegate next, IOptionsMonitor<ApiConfig> settings, ILogger<Authorization> logger)
{
    private readonly ApiConfig _apiConfig = settings.CurrentValue;

    public async Task Invoke(HttpContext httpContext)
    {
        string? authHeader = httpContext.Request.Headers["Authorization-Mego"];

        if (authHeader != null && authHeader.StartsWith( "Auth-Mego" ))
        {
            var encodeAuthorization = authHeader["Auth-Mego ".Length..].Trim();

            if (encodeAuthorization.Equals( _apiConfig.auth_ws ))
            {
                await next( httpContext );
            }
            else
            {
                httpContext.Response.ContentType = "application/json; charset=UTF-8";
                httpContext.Response.StatusCode = Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized );
                var result = ResponseError.ResException( "Credenciales incorrectas ws", Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized ), System.Net.HttpStatusCode.Unauthorized.ToString() );
                logger.LogError( "Authorization: {Result}", result );
                await httpContext.Response.WriteAsync( result );
            }
        }
        else
        {
            httpContext.Response.ContentType = "application/json; charset=UTF-8";
            httpContext.Response.StatusCode = Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized );
            var result = ResponseError.ResException( "No autorizado ws", Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized ), System.Net.HttpStatusCode.Unauthorized.ToString() );
            logger.LogError( "Authorization: {Result}", result );
            await httpContext.Response.WriteAsync( result );
        }
    }
}

public static class AuthorizationExtensions
{
    public static void UseAuthorizationMego(this IApplicationBuilder app)
    {
        app.UseMiddleware<Authorization>();
    }
}