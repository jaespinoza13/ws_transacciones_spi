using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Opis.Queries.Imprimir.OrdenPago.Common;
using Application.Persistence;
using Domain.Entities.Opis;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Features.Opis.Queries.Imprimir.OrdenPago;

public class ImprimirOrdenPagoHandler : IRequestHandler<ReqImprimirOrdenPago, ResImprimirOrdenPago>
{
    private readonly IOpisDat _opisDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<ImprimirOrdenPagoHandler> _logger;
    private readonly ApiSettings _settings;
    
    public ImprimirOrdenPagoHandler(IOpisDat opisDat, ILogs logs, ILogger<ImprimirOrdenPagoHandler> logger, IOptionsMonitor<ApiSettings> settings)
    {
        _opisDat = opisDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
        _settings = settings.CurrentValue;
    }

    
    public async Task<ResImprimirOrdenPago> Handle(ReqImprimirOrdenPago request, CancellationToken cancellationToken)
    {
        var respuesta = new ResImprimirOrdenPago();

        const string strOperacion = "GET_DETALLE_OPI";
        try
        {
            respuesta.LlenarResHeader( request );

            _ = _logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );

            var respuestaTransaccion = await _opisDat.ImprimirOrden( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var detalleOpi = Conversions.ConvertToClassDynamic<DetalleOpi>( (ConjuntoDatos)respuestaTransaccion.cuerpo );
                
                
                var bytes = request.str_tipo_ordenante switch
                {
                    "PROVEEDOR" => Autorizacion.GenerarOrdenPago( detalleOpi, _settings ),
                    _ => Autorizacion.GenerarInterbancaria( detalleOpi, _settings )
                };
                
                if (bytes.Length > 0)
                {
                    respuesta.autorizacion.file_bytes = bytes;
                    respuesta.autorizacion.str_doc_extencion = "application/pdf";
                    respuesta.str_res_codigo = "000";
                    respuesta.str_res_info_adicional = "Autorización generada correctamente";
                }
                else
                {
                    respuesta.autorizacion.file_bytes = null;
                    respuesta.autorizacion.str_doc_extencion = null;
                    respuesta.str_res_codigo = "999";
                    respuesta.str_res_info_adicional = "Error al generar la autorización";
                }
                
            }
            
            _ = _logs.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e );
            _logger.LogError( e, "Error en ImprimirOrdenPagoHandler" );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}