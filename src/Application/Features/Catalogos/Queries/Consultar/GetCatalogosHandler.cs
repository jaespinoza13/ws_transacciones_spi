using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.Catalogos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Catalogos.Queries.Consultar;

public class GetCatalogosHandler : IRequestHandler<ReqGetCatalogos, ResGetCatalogos>
{
    private readonly ICatalogosDat _catalogosDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<GetCatalogosHandler> _logger;

    public GetCatalogosHandler(ICatalogosDat catalogosDat, ILogs logs, ILogger<GetCatalogosHandler> logger)
    {
        _catalogosDat = catalogosDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
    }

    public async Task<ResGetCatalogos> Handle(ReqGetCatalogos request, CancellationToken cancellationToken)
    {
        var respuesta = new ResGetCatalogos();

        const string strOperacion = "GET_CATALOGOS";

        try
        {
            respuesta.LlenarResHeader( request );

            _ = _logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );

            var respuestaTransaccion = await _catalogosDat.GetCatalogos( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var ienumOpis = Conversions.ConvertToListClassDynamic<Catalogo>( (ConjuntoDatos)respuestaTransaccion.cuerpo );

                respuesta.lst_catalogos = (List<Catalogo>)ienumOpis;
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["Error"];
            _ = _logs.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e );
            _logger.LogError( e, "Error en GetCatalogosHandler" );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}