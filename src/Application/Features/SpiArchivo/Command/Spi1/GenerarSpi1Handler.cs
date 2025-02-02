﻿using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.SpiArchivo.Command.Spi1.Common;
using Application.Persistence;
using Domain.Entities.SpiArchivo;
using Newtonsoft.Json;


namespace Application.Features.SpiArchivo.Command.Spi1;

public class GenerarSpi1Handler(ISpiArchivoDat archivoDat, ILogs logs, ILogger<GenerarSpi1Handler> logger, IOptionsMonitor<ApiConfig> apiConfig) : IRequestHandler<ReqGenerarSpi1, ResGenerarSpi1>
{
    private readonly ApiConfig _apiConfig = apiConfig.CurrentValue;


    public async Task<ResGenerarSpi1> Handle(ReqGenerarSpi1 request, CancellationToken cancellationToken)
    {
        var respuesta = new ResGenerarSpi1();
        
        const string strOperacion = "SET_GENERAR_SPI1";

        try
        {
            respuesta.LlenarResHeader(request);
            
            logger.LogInformation("SET_GENERAR_SPI1.REQUEST: {request}", JsonConvert.SerializeObject(request));
            _ = logs.SaveHeaderLogs(request, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

            var resGenerarSpi1Dat = await archivoDat.GenerarSpi1( request );

            if (resGenerarSpi1Dat.codigo != "000")
            {
                respuesta.str_res_codigo = resGenerarSpi1Dat.codigo;
                respuesta.str_res_info_adicional = resGenerarSpi1Dat.diccionario["str_error"];
                
                logger.LogError("SET_GENERAR_SPI1.ERROR: {respuestaTransaccion}", JsonConvert.SerializeObject(resGenerarSpi1Dat));
                _ = logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
                
                return respuesta;
            }

            var body = resGenerarSpi1Dat.cuerpo;
            var cabeceraSpi = Conversions.ConvertToClass<CabeceraSpi1>( (ConjuntoDatos)body );
            var detalleSpi1 = Conversions.ConvertToList<DetalleSpi1>( (ConjuntoDatos)body, 1 ).ToList();
            var consolidadoSpi1 = Conversions.ConvertToList<DetalleSpi1Consolidado>( (ConjuntoDatos)body, 2 ).ToList();
            
            var (fileContents, md5Hash) = await Spi1Utils.GenerateSpi1TxtAndMd5(cabeceraSpi, detalleSpi1);

            respuesta.str_nombre_archivo = cabeceraSpi.str_nom_archivo_spi1;
            respuesta.int_cantidad_registros = cabeceraSpi.int_num_total_opi;
            respuesta.int_numero_corte = cabeceraSpi.int_numero_envio;
            respuesta.spi1_txt = Convert.ToBase64String( fileContents );
            respuesta.spi1_md5 = Convert.ToBase64String( Encoding.UTF8.GetBytes( md5Hash ) );
            
            var resConsolidado = Spi1Utils.ReporteConsolidado( _apiConfig, request.str_email, cabeceraSpi, consolidadoSpi1, fileContents );

            respuesta.str_res_codigo = resConsolidado.codigo;
            respuesta.str_res_info_adicional = resConsolidado.diccionario["str_error"];
                
            logger.LogInformation("SET_GENERAR_SPI1.RESPONSE: {respuesta}", JsonConvert.SerializeObject(respuesta));
            _ = logs.SaveResponseLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            
        }
        catch (Exception e)
        {
            respuesta.str_res_codigo = "003";
            respuesta.str_res_info_adicional = e.Message;
            
            logger.LogError("SET_GENERAR_SPI1.EXCEPTION: {e}", e);
            _ = logs.SaveExceptionLogs(respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, e);
            
            throw new ArgumentException( respuesta.str_id_transaccion );
        }
        return respuesta;
    }
}