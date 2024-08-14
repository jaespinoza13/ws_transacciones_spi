using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Models;
using Domain.Entities.SpiArchivo;

namespace Application.Features.SpiArchivo.Command.Spi1.Common;

public static class Spi1Utils
{
    public async static Task<(byte[] fileContents, string md5Hash)> GenerateSpi1TxtAndMd5(CabeceraSpi1 cabeceraSpi1, IEnumerable<DetalleSpi1> lstDetalleSpi)
    {
        using var stream = new MemoryStream();
        await using var writer = new StreamWriter( stream );

        var cabeceraSpi1Txt = $"{cabeceraSpi1.dtt_fecha_envio:dd/MM/yyyy HH:mm:ss},{cabeceraSpi1.int_codigo_spi1},{cabeceraSpi1.int_num_total_opi},{cabeceraSpi1.dec_monto_total_opi},{cabeceraSpi1.int_num_control},{cabeceraSpi1.str_num_cuenta_io}";

        await writer.WriteAsync( cabeceraSpi1Txt );
        await writer.WriteLineAsync();

        foreach (var detalleSpiTxt in lstDetalleSpi.Select( d =>
                     $"{d.dtt_fecha_registro_io:dd/MM/yyyy HH:mm:ss},{d.str_num_referencia},{d.int_cod_origen_io},{d.int_cod_moneda},{d.dec_monto_credito_opi},{d.int_cod_concepto_opi},{d.str_cuenta_bce_ir},{d.str_cuenta_co_io},{d.int_tipo_cuenta_co},{d.str_nombre_co},{d.str_lugar_opi_io},{d.str_cuenta_cb_ir},{d.int_tipo_cuenta_cb},{d.str_nombre_cb},{d.str_info_adicional_opi},{d.str_ced_ruc_cb}" ))
        {
            await writer.WriteLineAsync( detalleSpiTxt );
        }

        await writer.FlushAsync();

        var fileContents = stream.ToArray();

        var md5Hash = CalcularMd5( fileContents );

        return (fileContents, md5Hash);
    }

    private static string CalcularMd5(byte[] fileContents)
    {
        var md5HashBytes = MD5.HashData( fileContents );
        var md5Hash = BitConverter.ToString( md5HashBytes ).Replace( "-", "" ).ToLower();
        return md5Hash;
    }

    public static RespuestaTransaccion ReporteConsolidado(ApiConfig config, string email, CabeceraSpi1 cabeceraSpi1, IReadOnlyList<DetalleSpi1Consolidado> listConsolidado, byte[] bytesSpi1)
    {
        var respuesta = new RespuestaTransaccion();
        var mailMessage = new MailMessage( email, email );
        mailMessage.IsBodyHtml = true;
        mailMessage.Subject = "\ud83d\udc4d\ud83d\ude09 CoopMego Reporte Consolidado Spi1";

        var memoryStreamSpi1Txt = new MemoryStream();
        memoryStreamSpi1Txt.Write( bytesSpi1, 0, bytesSpi1.Length );
        memoryStreamSpi1Txt.Position = 0;

        var valorMd5 = CalcularMd5( bytesSpi1 );
        var memoryStreamSpi1Md5 = new MemoryStream( Encoding.UTF8.GetBytes( valorMd5 ) );

        var adjuntoSpi1Txt =
            new Attachment( memoryStreamSpi1Txt, $"{cabeceraSpi1.str_nom_archivo_spi1}.txt", "text/plain" );
        var adjuntoSpiMd5 = new Attachment( memoryStreamSpi1Md5, $"{cabeceraSpi1.str_nom_archivo_spi1}.MD5" );

        mailMessage.Attachments.Add( adjuntoSpi1Txt );
        mailMessage.Attachments.Add( adjuntoSpiMd5 );

        var body = Plantilla.GenerarPlantillaConsolidado( cabeceraSpi1, listConsolidado );

        mailMessage.Body = body;

        var clienteSmtp = new SmtpClient( config.server_interno_email, int.Parse( config.puerto_interno_email ) );

        try
        {
            clienteSmtp.Send( mailMessage );
            respuesta.codigo = "000";
            respuesta.diccionario.Add( "str_error", "Consolidado enviado correctamente \ud83d\udc4d\ud83d\ude09" );
        }
        catch (Exception ex)
        {
            respuesta.codigo = "999";
            respuesta.diccionario.Add( "str_error", ex.InnerException?.Message ?? ex.Message );
        }
        finally
        {
            mailMessage.Dispose();
            adjuntoSpi1Txt.Dispose();
            adjuntoSpiMd5.Dispose();
            memoryStreamSpi1Txt.Dispose();
            memoryStreamSpi1Md5.Dispose();
            clienteSmtp.Dispose();
        }

        return respuesta;
    }
}