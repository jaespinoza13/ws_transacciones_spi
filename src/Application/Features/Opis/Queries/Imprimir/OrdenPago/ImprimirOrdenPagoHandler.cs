using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Opis.Queries.Imprimir.OrdenPago.Common;
using Application.Persistence;
using Domain.Entities.Opis;
using Newtonsoft.Json;


namespace Application.Features.Opis.Queries.Imprimir.OrdenPago;

public class ImprimirOrdenPagoHandler(IOpisDat opisDat, ILogs logs, ILogger<ImprimirOrdenPagoHandler> logger, IOptionsMonitor<ApiConfig> apiConfig) : IRequestHandler<ReqImprimirOrdenPago, ResImprimirOrdenPago>
{
    private readonly ApiConfig _apiConfig = apiConfig.CurrentValue;
    
    public async Task<ResImprimirOrdenPago> Handle(ReqImprimirOrdenPago request, CancellationToken cancellationToken)
    {
        var respuesta = new ResImprimirOrdenPago();

        const string strOperacion = "GET_IMPRIMIR_ORDEN";
        try
        {
            respuesta.LlenarResHeader(request);

            logger.LogInformation("GET_IMPRIMIR_ORDEN.REQUEST: {request}", JsonConvert.SerializeObject(request));
            _ = logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

            var respuestaTransaccion = await opisDat.ImprimirOrden(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var detalleOpi = Conversions.ConvertToClass<DetalleOpi>((ConjuntoDatos)respuestaTransaccion.cuerpo);


                var bytes = request.str_tipo_ordenante switch
                {
                    "PROVEEDOR" => Autorizacion.GenerarOrdenPago(detalleOpi, _apiConfig, request.str_tipo_documento),
                    _ => Autorizacion.GenerarInterbancaria(detalleOpi, _apiConfig)
                };

                if (bytes.Length > 0)
                {
                    respuesta.file_bytes = bytes;
                    respuesta.str_doc_extencion = "application/pdf";
                    respuesta.str_res_codigo = "000";
                    respuesta.str_res_info_adicional = "Autorización generada correctamente";
                }
                else
                {
                    respuesta.file_bytes = null;
                    respuesta.str_doc_extencion = null;
                    respuesta.str_res_codigo = "999";
                    respuesta.str_res_info_adicional = "Error al generar la autorización";
                }
            }

            logger.LogInformation("GET_IMPRIMIR_ORDEN.RESPONSE: {respuesta}", JsonConvert.SerializeObject(respuesta));
            _ = logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            
        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;
            
            logger.LogError("GET_IMPRIMIR_ORDEN.EXCEPTION: {e}", e);
            _ = logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e);
            
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}