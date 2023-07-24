using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.Consultas;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Consultas.Queries.Entidades;

public class EntidadesHandler: IRequestHandler<ReqEntidades, ResEntidades>
{
    private readonly IConsultaDat _consultaDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<EntidadesHandler> _logger;

    public EntidadesHandler(IConsultaDat consultaDat, ILogs logs, ILogger<EntidadesHandler> logger)
    {
        _consultaDat = consultaDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
    }
    public async Task<ResEntidades> Handle(ReqEntidades request, CancellationToken cancellationToken)
    {
        var respuesta = new ResEntidades();

        const string strOperacion = "GET_ENTIDADES_FINANCIERAS";

        try
        {
            respuesta.LlenarResHeader( request );

            _ = _logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );

            var respuestaTransaccion = await _consultaDat.BuscarEntidades( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var ienumOpis = Conversions.ConvertToListClassDynamic<EntidadFinanciera>( (ConjuntoDatos)respuestaTransaccion.cuerpo );

                respuesta.lst_entidades = (List<EntidadFinanciera>)ienumOpis;
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["Error"];
            _ = _logs.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e );
            _logger.LogError( e, "Error en EntidadesHandler" );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}