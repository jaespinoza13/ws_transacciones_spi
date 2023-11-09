using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities;


namespace Application.Features.Catalogos.Queries.GetCatalogos;

public class GetCatalogoHandler : IRequestHandler<ReqGetCatalogo, ResGetCatalogo>
{
    private readonly ICatalogoDat _catalogoDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<GetCatalogoHandler> _logger;

    public GetCatalogoHandler(ILogger<GetCatalogoHandler> logger, ILogs logs, ICatalogoDat parametroDat)
    {
        _logger = logger;
        _logs = logs;
        _catalogoDat = parametroDat;
        _clase = GetType().FullName!;
    }

    public async Task<ResGetCatalogo> Handle(ReqGetCatalogo request, CancellationToken cancellationToken)
    {
        var respuesta = new ResGetCatalogo();

        const string strOperacion = "GET_CATALOGOS_SPI";

        try
        {
            respuesta.LlenarResHeader(request);

            _ = _logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);

            var respuestaTransaccion = await _catalogoDat.GetCatalogos(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var body = (ConjuntoDatos)respuestaTransaccion.cuerpo;
                respuesta.lst_catalogos  = Conversions.ConvertToList<Catalogo>( body ).ToList();
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
            _logger.LogError(e, "Ocurrió un error en GetCatalogoHandler");
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}