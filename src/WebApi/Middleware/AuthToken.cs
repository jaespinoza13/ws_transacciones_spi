using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Middleware.Common;

namespace WebApi.Middleware;

public class AuthToken(RequestDelegate next, ILogger<AuthToken> logger)
{
    public async Task Invoke(HttpContext httpContext)
    {
        var authHeader = httpContext.Request.Headers.ContainsKey( "Authorization" );

        var authHeaderBearer = httpContext.Request.Headers.Authorization.ToString().StartsWith( "Bearer " );

        if (!authHeader || !authHeaderBearer)
        {
            var result = ResponseError.ResException( "Petición inválida token requerido ws", Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized ), System.Net.HttpStatusCode.Unauthorized.ToString() );
            httpContext.Response.ContentType = "application/json; charset=UTF-8";
            httpContext.Response.StatusCode = Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized );
            logger.LogError( "Authorization: {Result}", result );
            await httpContext.Response.WriteAsync( result );

            return;
        }

        var authenticationResult = await httpContext.AuthenticateAsync( JwtBearerDefaults.AuthenticationScheme );

        if (!authenticationResult.Succeeded)
        {
            var result = ResponseError.ResException( "Token inválido ws", Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized ), System.Net.HttpStatusCode.Unauthorized.ToString() );
            httpContext.Response.ContentType = "application/json; charset=UTF-8";
            httpContext.Response.StatusCode = Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized );
            logger.LogError( "Authorization: {Result}", result );
            await httpContext.Response.WriteAsync( result );
            return;
        }

        await next( httpContext );
    }
}

public static class AuthTokenExtensions
{
    public static void UseJwtTokenMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<AuthToken>();
    }
}