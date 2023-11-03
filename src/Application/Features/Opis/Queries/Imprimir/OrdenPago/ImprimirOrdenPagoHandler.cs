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


namespace Application.Features.Opis.Queries.Imprimir.OrdenPago;

public class ImprimirOrdenPagoHandler : IRequestHandler<ReqImprimirOrdenPago, ResImprimirOrdenPago>
{
    private readonly IOpisDat _opisDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<ImprimirOrdenPagoHandler> _logger;
    private readonly ApiConfig _apiConfig;

    public ImprimirOrdenPagoHandler(IOpisDat opisDat, ILogs logs, ILogger<ImprimirOrdenPagoHandler> logger, IOptionsMonitor<ApiConfig> apiConfig)
    {
        _opisDat = opisDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
        _apiConfig = apiConfig.CurrentValue;
    }


    public async Task<ResImprimirOrdenPago> Handle(ReqImprimirOrdenPago request, CancellationToken cancellationToken)
    {
        var respuesta = new ResImprimirOrdenPago();

        const string strOperacion = "GET_DETALLE_OPI";
        try
        {
            respuesta.LlenarResHeader(request);

            _ = _logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);

            var respuestaTransaccion = await _opisDat.ImprimirOrden(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var detalleOpi =
                    Conversions.ConvertToClass<DetalleOpi>((ConjuntoDatos)respuestaTransaccion.cuerpo);


                var bytes = request.str_tipo_ordenante switch
                {
                    "PROVEEDOR" => Autorizacion.GenerarOrdenPago(detalleOpi, _apiConfig),
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

            _ = _logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e);
            _logger.LogError(e, "Ocurrió un error en ImprimirOrdenPagoHandler");
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}