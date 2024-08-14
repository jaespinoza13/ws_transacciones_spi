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
using Newtonsoft.Json;


namespace Application.Features.Opis.Queries.Imprimir.Transferencias;

public class ImprimirTransferenciaHandler(IOpisDat opisDat, ILogs logs, ILogger<ImprimirTransferenciaHandler> logger, IOptionsMonitor<ApiConfig> apiConfig) : IRequestHandler<ReqImprimirTransferencia, ResImprimirTransferencia>
{
    private readonly ApiConfig _apiConfig = apiConfig.CurrentValue;

    public async Task<ResImprimirTransferencia> Handle(ReqImprimirTransferencia request, CancellationToken cancellationToken)
    {
        var respuesta = new ResImprimirTransferencia();

        const string strOperacion = "GET_IMPRIMIR_TRANSFERENCIA";

        try
        {
            respuesta.LlenarResHeader( request );

            logger.LogInformation( "GET_IMPRIMIR_TRANSFERENCIA.REQUEST: {request}", JsonConvert.SerializeObject( request ) );
            _ = logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name );

            var respuestaTransaccion = await opisDat.ImprimirTransferencia( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var autorizacion = Conversions.ConvertToClass<AutorizacionTransfExterna>( (ConjuntoDatos)respuestaTransaccion.cuerpo );

                var bytes = request.str_tipo_persona switch
                {
                    "J" => Autorizacion.GenerarAutorizacionPersonaJuridica( autorizacion, _apiConfig ),
                    _ => Autorizacion.GenerarAutorizacionPersonaNatural( autorizacion, _apiConfig )
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

            logger.LogInformation( "GET_IMPRIMIR_TRANSFERENCIA.RESPONSE: {respuesta}", JsonConvert.SerializeObject( respuesta ) );
            _ = logs.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name );

        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;

            logger.LogError( "GET_IMPRIMIR_TRANSFERENCIA.EXCEPTION: {e}", e );
            _ = logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e );

            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}