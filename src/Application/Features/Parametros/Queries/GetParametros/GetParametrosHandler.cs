using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Parametros.Queries.GetParametros;

public class GetParametrosHandler : IRequestHandler<ReqGetParametros, ResGetParametros>
{
    private readonly IParametroDat _parametroDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<GetParametrosHandler> _logger;

    public GetParametrosHandler(ILogger<GetParametrosHandler> logger, ILogs logs, IParametroDat parametroDat)
    {
        _logger = logger;
        _logs = logs;
        _parametroDat = parametroDat;
        _clase = GetType().FullName!;
    }

    public async Task<ResGetParametros> Handle(ReqGetParametros request, CancellationToken cancellationToken)
    {
        var respuesta = new ResGetParametros();

        const string strOperacion = "GET_PARAMETROS";

        try
        {
            respuesta.LlenarResHeader(request);

            _ = _logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);

            var respuestaTransaccion = await _parametroDat.GetParametros(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var parametros = Conversions.ConvertToListClassDynamic<Parametro>((ConjuntoDatos)respuestaTransaccion.cuerpo);

                respuesta.lst_parametros = (List<Parametro>)parametros;
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            _ = _logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e);
            _logger.LogError(e, "Error en GetParametrosHandler");
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}