using Application.Common.Interfaces;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace WebUI.Filters;

public class SessionControlFilter : IActionFilter
{
    private readonly ISessionControl _session;
    public SessionControlFilter(ISessionControl sessionControl) => _session = sessionControl;

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var estadoSesion = Convert.ToInt32( context.HttpContext.Request.Headers["int_estado"] );
        if (estadoSesion == -1) return;
        var model = context.ActionArguments.First();
        var raw = JsonSerializer.Deserialize<ValidaSesion>( JsonSerializer.Serialize( model.Value ) )!;
        raw.int_estado = estadoSesion;
        var respuesta = _session.SessionControlFilter( raw );

        if (respuesta.codigo.Equals( "000" )) return;
        var resException = new ResException
        {
            str_res_codigo = respuesta.codigo,
            str_res_id_servidor = "Sesion Caducada",
            str_res_estado_transaccion = "ERR",
            dt_res_fecha_msj_crea = DateTime.Now,
            str_res_info_adicional = respuesta.diccionario["str_error"]
        };

        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
        context.Result = new ObjectResult( resException );
    }
}