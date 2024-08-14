using iText.Html2pdf;
using iText.Kernel.Pdf;
using Application.Common.Models;
using Domain.Entities.Opis;

namespace Application.Features.Opis.Queries.Imprimir.Transferencias.Common;

public static class Autorizacion
{
    public static string GenerarAutorizacionPersonaNatural(AutorizacionTransfExterna autorizacion, ApiConfig apiConfig)
    {
        var pathPlantilla = $"{Directory.GetCurrentDirectory()}/{apiConfig.path_template_autorizacion_pn}";
        var pathLogo = $"{Directory.GetCurrentDirectory()}/{apiConfig.path_logo_png}";
        
        var plantillaContent = File.ReadAllText( pathPlantilla );
        plantillaContent = plantillaContent.Replace( "$str_logo$", pathLogo );
        plantillaContent = plantillaContent.Replace( "$str_mensaje$", autorizacion.str_mensaje );
        plantillaContent = plantillaContent.Replace( "$dtt_fecha$", DateTime.Now.ToString( "dd/MM/yyyy HH:mm" ) );
        plantillaContent = plantillaContent.Replace( "$int_codigo_opi$", autorizacion.int_codigo_opi.ToString() );
        plantillaContent = plantillaContent.Replace( "$str_num_cuenta_ord$", autorizacion.str_num_cuenta_ord );
        plantillaContent = plantillaContent.Replace( "$str_num_cuenta_benef$", autorizacion.str_num_cuenta_benef );
        plantillaContent = plantillaContent.Replace( "$str_tipo_cuenta_benef$", autorizacion.str_tipo_cuenta_benef );
        plantillaContent = plantillaContent.Replace( "$str_banco_benef$", autorizacion.str_banco_benef );
        plantillaContent = plantillaContent.Replace( "$str_razon_social_benef$", autorizacion.str_razon_social_benef );
        plantillaContent = plantillaContent.Replace( "$str_cedula_ruc_benef$", autorizacion.str_cedula_ruc_benef );
        plantillaContent = plantillaContent.Replace( "$str_monto_opi$", autorizacion.str_monto_opi );
        plantillaContent = plantillaContent.Replace( "$str_observaciones$", autorizacion.str_observaciones );
        plantillaContent = plantillaContent.Replace( "$str_concepto$", autorizacion.str_concepto );
        
        plantillaContent = plantillaContent.Replace( "$str_nombre_ord$", autorizacion.str_nombre_ord );
        plantillaContent = plantillaContent.Replace( "$str_usuario$", autorizacion.str_usuario );
        plantillaContent = plantillaContent.Replace( "$str_oficina$", autorizacion.str_oficina );
        
        using var pdfMemoryStream = new MemoryStream();
        
        var pdfWriter = new PdfWriter( pdfMemoryStream );

        var pdfDocument = new PdfDocument( pdfWriter );

        var properties = new ConverterProperties();

        HtmlConverter.ConvertToPdf( plantillaContent, pdfDocument, properties );

        pdfDocument.Close();

        var pdfBytes = pdfMemoryStream.ToArray();

        var base64Pdf = Convert.ToBase64String( pdfBytes, Base64FormattingOptions.None );
        
        pdfMemoryStream.Close();
        
        return base64Pdf;
    }
     public static string GenerarAutorizacionPersonaJuridica(AutorizacionTransfExterna autorizacion, ApiConfig apiConfig)
    {
        var pathPlantilla = $"{Directory.GetCurrentDirectory()}/{apiConfig.path_template_autorizacion_pj}";
        var pathLogo = $"{Directory.GetCurrentDirectory()}/{apiConfig.path_logo_png}";
        
        var plantillaContent = File.ReadAllText( pathPlantilla );
        plantillaContent = plantillaContent.Replace( "$str_logo$", pathLogo );
        plantillaContent = plantillaContent.Replace( "$str_mensaje$", autorizacion.str_mensaje );
        plantillaContent = plantillaContent.Replace( "$dtt_fecha$", DateTime.Now.ToString( "dd/MM/yyyy HH:mm" ) );
        plantillaContent = plantillaContent.Replace( "$int_codigo_opi$", autorizacion.int_codigo_opi.ToString() );
        plantillaContent = plantillaContent.Replace( "$str_num_cuenta_ord$", autorizacion.str_num_cuenta_ord );
        plantillaContent = plantillaContent.Replace( "$str_num_cuenta_benef$", autorizacion.str_num_cuenta_benef );
        plantillaContent = plantillaContent.Replace( "$str_tipo_cuenta_benef$", autorizacion.str_tipo_cuenta_benef );
        plantillaContent = plantillaContent.Replace( "$str_banco_benef$", autorizacion.str_banco_benef );
        plantillaContent = plantillaContent.Replace( "$str_razon_social_benef$", autorizacion.str_razon_social_benef );
        plantillaContent = plantillaContent.Replace( "$str_cedula_ruc_benef$", autorizacion.str_cedula_ruc_benef );
        plantillaContent = plantillaContent.Replace( "$str_monto_opi$", autorizacion.str_monto_opi );
        plantillaContent = plantillaContent.Replace( "$str_observaciones$", autorizacion.str_observaciones );
        plantillaContent = plantillaContent.Replace( "$str_concepto$", autorizacion.str_concepto );
        
        plantillaContent = plantillaContent.Replace( "$str_nombre_ord$", autorizacion.str_nombre_ord );
        plantillaContent = plantillaContent.Replace( "$str_usuario$", autorizacion.str_usuario );
        plantillaContent = plantillaContent.Replace( "$str_oficina$", autorizacion.str_oficina );
        
        using var pdfMemoryStream = new MemoryStream();
        
        var pdfWriter = new PdfWriter( pdfMemoryStream );

        var pdfDocument = new PdfDocument( pdfWriter );

        var properties = new ConverterProperties();

        HtmlConverter.ConvertToPdf( plantillaContent, pdfDocument, properties );

        pdfDocument.Close();

        var pdfBytes = pdfMemoryStream.ToArray();

        var base64Pdf = Convert.ToBase64String( pdfBytes, Base64FormattingOptions.None );
        
        pdfMemoryStream.Close();
        
        return base64Pdf;
    }
}