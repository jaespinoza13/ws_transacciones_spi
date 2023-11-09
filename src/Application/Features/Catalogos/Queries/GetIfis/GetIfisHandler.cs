using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Catalogos.Queries.GetIfis;

public class GetIfisHandler: IRequestHandler<ReqGetIfis, ResGetIfis>
{
    private readonly ICatalogoDat _catalogoDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<GetIfisHandler> _logger;
    
    public GetIfisHandler(ILogger<GetIfisHandler> logger, ILogs logs, ICatalogoDat parametroDat)
    {
        _logger = logger;
        _logs = logs;
        _catalogoDat = parametroDat;
        _clase = GetType().FullName!;
    }
    
    public async Task<ResGetIfis> Handle(ReqGetIfis request, CancellationToken cancellationToken)
    {
        var respuesta = new ResGetIfis();

        const string strOperacion = "GET_IFIS_SPI";

        try
        {
            respuesta.LlenarResHeader(request);

            _ = _logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);

            var respuestaTransaccion = await _catalogoDat.GetIfis(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var body = (ConjuntoDatos)respuestaTransaccion.cuerpo;
                respuesta.lst_ifis  = Conversions.ConvertToList<Ifi>( body ).ToList();
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            _ = _logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);
        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;
            _ = _logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e);
            _logger.LogError(e, "Ocurrió un error en GetIfisHandler");
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}