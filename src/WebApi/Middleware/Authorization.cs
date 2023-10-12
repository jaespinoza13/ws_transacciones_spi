using System.Text.Json;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Microsoft.Extensions.Options;

namespace WebApi.Middleware;

public class Authorization
{
    private readonly RequestDelegate _next;
    private readonly ApiConfig _apiConfig;

    public Authorization(RequestDelegate next, IOptionsMonitor<ApiConfig> settings)
    {
        _next = next;
        _apiConfig = settings.CurrentValue;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        string? authHeader = httpContext.Request.Headers["Authorization-Mego"];

        if (authHeader != null && authHeader.StartsWith("Auth-Mego"))
        {
            var encodeAuthorization = authHeader["Auth-Mego ".Length..].Trim();

            if (encodeAuthorization.Equals(_apiConfig.auth_ws))
            {
                await _next(httpContext);
            }
            else
            {
                await ResException(httpContext, "Credenciales incorrectas WS", Convert.ToInt32(System.Net.HttpStatusCode.Unauthorized), System.Net.HttpStatusCode.Unauthorized.ToString());
            }
        }
        else
        {
            await ResException(httpContext, "No autorizado WS", Convert.ToInt32(System.Net.HttpStatusCode.Unauthorized), System.Net.HttpStatusCode.Unauthorized.ToString());
        }
    }

    private static async Task ResException(HttpContext httpContext, string infoAdicional, int statusCode, string strResIdServidor)
    {
        httpContext.Response.ContentType = "application/json; charset=UTF-8";
        httpContext.Response.StatusCode = statusCode;

        var respuesta = new ResException
        {
            str_res_id_servidor = strResIdServidor,
            str_res_codigo = statusCode.ToString(),
            dt_res_fecha_msj_crea = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null),
            str_res_estado_transaccion = "ERR",
            str_res_info_adicional = infoAdicional
        };

        var result = JsonSerializer.Serialize(respuesta);

        await httpContext.Response.WriteAsync(result);
    }
}
public static class AuthorizationExtensions
{
    public static void UseAuthorizationMego(this IApplicationBuilder app)
    {
        app.UseMiddleware<Authorization>();
    }
}
