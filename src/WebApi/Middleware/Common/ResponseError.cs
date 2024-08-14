using Application.Common.ISO20022.Models;
using Newtonsoft.Json;

namespace WebApi.Middleware.Common;

public  static class ResponseError
{
    public static string ResException(string infoAdicional, int statusCode, string strResIdServidor)
    {
        var respuesta = new ResException
        {
            str_res_id_servidor = strResIdServidor,
            str_res_codigo = statusCode.ToString(),
            dt_res_fecha_msj_crea = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null),
            str_res_estado_transaccion = "ERR",
            str_res_info_adicional = infoAdicional
        };

        return JsonConvert.SerializeObject(respuesta);
        
    }
}