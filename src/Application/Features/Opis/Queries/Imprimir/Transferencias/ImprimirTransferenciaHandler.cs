using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Opis.Queries.Imprimir.Transferencias.Common;
using Application.Persistence;
using Domain.Entities.Opis;


namespace Application.Features.Opis.Queries.Imprimir.Transferencias;

public class ImprimirTransferenciaHandler : IRequestHandler<ReqImprimirTransferencia, ResImprimirTransferencia>
{
    private readonly IOpisDat _opisDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<ImprimirTransferenciaHandler> _logger;
    private readonly ApiConfig _apiConfig;

    public ImprimirTransferenciaHandler(IOpisDat opisDat, ILogs logs, ILogger<ImprimirTransferenciaHandler> logger, IOptionsMonitor<ApiConfig> apiConfig)
    {
        _opisDat = opisDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
        _apiConfig = apiConfig.CurrentValue;
    }


    public async Task<ResImprimirTransferencia> Handle(ReqImprimirTransferencia request, CancellationToken cancellationToken)
    {
        var respuesta = new ResImprimirTransferencia();
        const string strOperacion = "GET_IMPRIMIR_TRANSFERENCIA";
        try
        {
            respuesta.LlenarResHeader(request);
            
            _ = _logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);

            var respuestaTransaccion = await _opisDat.ImprimirTransferencia(request);

            if (respuestaTransaccion.codigo.Equals("000"))
            {
                var autorizacion = Conversions.ConvertToClass<AutorizacionTransfExterna>((ConjuntoDatos)respuestaTransaccion.cuerpo);

                var bytes = request.str_tipo_persona switch
                {
                    "N" => Autorizacion.GenerarAutorizacionPersonaNatural(autorizacion, _apiConfig),
                    _ => Autorizacion.GenerarAutorizacionPersonaJuridica(autorizacion, _apiConfig)
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

            _ = _logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase);
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e);
            _logger.LogError(e, "Ocurrió un error en ImprimirTransferenciaHandler");
            throw new ArgumentException(respuesta.str_id_transaccion);
        }

        return respuesta;
    }
}