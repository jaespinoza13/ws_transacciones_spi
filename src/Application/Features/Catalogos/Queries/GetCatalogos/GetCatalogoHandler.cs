using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities;
using Newtonsoft.Json;


namespace Application.Features.Catalogos.Queries.GetCatalogos;

public class GetCatalogoHandler(ILogger<GetCatalogoHandler> logger, ILogs logs, ICatalogoDat parametroDat) : IRequestHandler<ReqGetCatalogo, ResGetCatalogo>
{
    public async Task<ResGetCatalogo> Handle(ReqGetCatalogo request, CancellationToken cancellationToken)
    {
        var respuesta = new ResGetCatalogo();
        
        const string strOperacion = "GET_CATALOGOS_SPI";

        try
        {
            respuesta.LlenarResHeader(request);

            logger.LogInformation("GET_CATALOGOS_SPI.REQUEST: {request}", JsonConvert.SerializeObject(request));
            _ = logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            
            var respuestaTransaccion = await parametroDat.GetCatalogos(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var body = (ConjuntoDatos)respuestaTransaccion.cuerpo;
                respuesta.lst_catalogos  = Conversions.ConvertToList<Catalogo>( body ).ToList();
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            
            logger.LogInformation("GET_CATALOGOS_SPI.RESPONSE: {respuesta}", JsonConvert.SerializeObject(respuesta));
            _ = logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;
            
            logger.LogError("GET_CATALOGOS_SPI.EXCEPTION: {e}", e);
            _ = logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e);

            
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}