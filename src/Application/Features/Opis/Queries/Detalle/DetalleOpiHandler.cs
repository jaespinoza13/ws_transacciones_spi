using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.Opis;
using Newtonsoft.Json;

namespace Application.Features.Opis.Queries.Detalle;

public class DetalleOpiHandler(IOpisDat opisDat, ILogs logs, ILogger<DetalleOpiHandler> logger) : IRequestHandler<ReqDetalleOpi, ResDetalleOpi>
{
    public async Task<ResDetalleOpi> Handle(ReqDetalleOpi request, CancellationToken cancellationToken)
    {
        var respuesta = new ResDetalleOpi();

        const string strOperacion = "GET_DETALLE_OPI";

        try
        {
            respuesta.LlenarResHeader( request );

            logger.LogInformation("GET_DETALLE_OPI.REQUEST: {request}", JsonConvert.SerializeObject(request));
            _ = logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

            var respuestaTransaccion = await opisDat.DetalleOpi( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var detalleOpi = Conversions.ConvertToClass<DetalleOpi>( (ConjuntoDatos)respuestaTransaccion.cuerpo );

                if (detalleOpi.str_tipo_ordenante.Equals( "CLIENTE" ))
                {
                    var condiciones = Conversions.ConvertToList<FirmanteCuenta>( (ConjuntoDatos)respuestaTransaccion.cuerpo, 1 );
                    respuesta.lst_condiciones = (List<FirmanteCuenta>)condiciones;
                }

                respuesta.detalle_opi = detalleOpi;
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            
            logger.LogInformation("GET_DETALLE_OPI.RESPONSE: {respuesta}", JsonConvert.SerializeObject(respuesta));
            _ = logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            
        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;
            
            logger.LogError("GET_DETALLE_OPI.EXCEPTION: {e}", e);
            _ = logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e);
            
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}