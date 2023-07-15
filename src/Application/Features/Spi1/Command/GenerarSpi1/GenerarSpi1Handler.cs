using MediatR;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.Spi1;

namespace Application.Features.Spi1.Command.GenerarSpi1;

public class GenerarSpi1Handler : IRequestHandler<ReqGenerarSpi1, ResGenerarSpi1>
{
    private readonly ISpi1Dat _spi1Dat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<GenerarSpi1Handler> _logger;

    public GenerarSpi1Handler(ISpi1Dat spi1Dat, ILogs logs, ILogger<GenerarSpi1Handler> logger)
    {
        _spi1Dat = spi1Dat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
    }


    public async Task<ResGenerarSpi1> Handle(ReqGenerarSpi1 request, CancellationToken cancellationToken)
    {
        var respuesta = new ResGenerarSpi1();
        const string strOperacion = "GET_ARCHIVO_SPI1";
        try
        {
            respuesta.LlenarResHeader( request );

            _ = _logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );

            var respuestaTransaccion = await _spi1Dat.GenerarSpi1( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var cabeceraSpi =
                    Conversions.ConvertToClassDynamic<CabeceraSpi1>( (ConjuntoDatos)respuestaTransaccion.cuerpo );
                var detalleSpi = Conversions.ConvertToListClassDynamic<DetalleSpi1>( (ConjuntoDatos)respuestaTransaccion.cuerpo, 1 );
                var lstDetalleSpi = (List<DetalleSpi1>)detalleSpi;

                using var stream = new MemoryStream();
                await using var writer = new StreamWriter( stream );
                var cabceraSpi1Txt = $"{cabeceraSpi.dtt_fecha_envio:dd/MM/yyyy HH:mm:ss},{cabeceraSpi.int_codigo_spi1},{cabeceraSpi.int_num_total_opi},{cabeceraSpi.dec_monto_total_opi},{cabeceraSpi.int_num_control},{cabeceraSpi.str_num_cuenta_io}";

                await writer.WriteAsync( cabceraSpi1Txt );
                await writer.WriteLineAsync();
                foreach (var detalleSpiTxt in lstDetalleSpi.Select( detalle => $"{detalle.dtt_fecha_registro_io:dd/MM/yyyy HH:mm:ss},{detalle.str_num_referencia},{detalle.int_cod_origen_io},{detalle.int_cod_moneda},{detalle.dec_monto_credito_opi},{detalle.int_cod_concepto_opi},{detalle.str_cuenta_bce_ir},{detalle.str_cuenta_co_io},{detalle.int_tipo_cuenta_co},{detalle.str_nombre_co},{detalle.str_lugar_opi_io},{detalle.str_cuenta_cb_ir},{detalle.int_tipo_cuenta_cb},{detalle.str_nombre_cb},{detalle.str_info_adicional_opi},{detalle.str_ced_ruc_cb}" ))
                {
                    await writer.WriteLineAsync( detalleSpiTxt );
                }

                await writer.FlushAsync();
                await writer.DisposeAsync();

                respuesta.spi1_txt = Convert.ToBase64String( stream.ToArray() );
                respuesta.spi1_md5 = Convert.ToBase64String( MD5.HashData( stream.ToArray() ) );

                using var zipStream = new MemoryStream();

                using (var spi1Txt = new ZipArchive( zipStream, ZipArchiveMode.Create, true ))
                {
                    var hashEntry = spi1Txt.CreateEntry( $"{cabeceraSpi.str_nom_archivo_spi1}.md5" );

                    var data = stream.ToArray();

                    var hash = MD5.HashData( data );

                    await using (var hashEntryStream = hashEntry.Open())
                    {
                        await hashEntryStream.WriteAsync( hash, cancellationToken );
                    }

                    var entry = spi1Txt.CreateEntry( $"{cabeceraSpi.str_nom_archivo_spi1}.txt" );

                    using var entryStream = new MemoryStream();

                    await entryStream.WriteAsync( data, cancellationToken );

                    entryStream.Seek( 0, SeekOrigin.Begin );

                    await entryStream.CopyToAsync( entry.Open(), cancellationToken );
                }
                zipStream.Seek( 0, SeekOrigin.Begin );

                using var finalZipStream = new MemoryStream();

                await zipStream.CopyToAsync( finalZipStream, cancellationToken );

                await using (var fileStream = new FileStream( $"{cabeceraSpi.str_nom_archivo_spi1}.zip", FileMode.Create ))
                {
                    finalZipStream.Seek( 0, SeekOrigin.Begin );

                    await finalZipStream.CopyToAsync( fileStream, cancellationToken );
                }
                respuesta.spi1_zip = Convert.ToBase64String( finalZipStream.ToArray() );
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["str_error"];
            _ = _logs.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e );
            _logger.LogError( e, "Error en GenerarSpi1Handler" );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}