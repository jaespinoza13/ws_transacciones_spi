using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Features.Catalogos.Queries.GetIfis;

public class GetIfisHandler(ILogger<GetIfisHandler> logger, ILogs logs, ICatalogoDat parametroDat) : IRequestHandler<ReqGetIfis, ResGetIfis>
{
    public async Task<ResGetIfis> Handle(ReqGetIfis request, CancellationToken cancellationToken)
    {
        var respuesta = new ResGetIfis();

        const string strOperacion = "GET_IFIS_SPI";

        try
        {
            respuesta.LlenarResHeader( request );

            logger.LogInformation( "GET_IFIS_SPI.REQUEST: {request}", JsonConvert.SerializeObject( request ) );
            _ = logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name );


            var respuestaTransaccion = await parametroDat.GetIfis( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var body = (ConjuntoDatos)respuestaTransaccion.cuerpo;
                respuesta.lst_ifis = Conversions.ConvertToList<Ifi>( body ).ToList();
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];

            logger.LogError( "GET_IFIS_SPI.RESPONSE: {respuestaTransaccion}", JsonConvert.SerializeObject( respuestaTransaccion ) );
            _ = logs.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name );

        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;

            logger.LogError( "GET_IFIS_SPI.EXCEPTION: {e}", e );
            _ = logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e );

            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}