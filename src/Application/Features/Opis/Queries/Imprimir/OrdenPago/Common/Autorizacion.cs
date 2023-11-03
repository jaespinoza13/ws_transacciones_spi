using iText.Html2pdf;
using iText.Kernel.Pdf;
using Application.Common.Models;
using Domain.Entities.Opis;

namespace Application.Features.Opis.Queries.Imprimir.OrdenPago.Common;

public static class Autorizacion
{
    public static string GenerarOrdenPago(DetalleOpi orden, ApiConfig apiConfig)
    {
        const string formatMoney = "0.00";
        var pathPlantilla = $"{Directory.GetCurrentDirectory()}/{apiConfig.path_template_autorizacion_proveedor}";
        var plantillaContent = File.ReadAllText( pathPlantilla );
        plantillaContent = plantillaContent.Replace( "$str_logo$", apiConfig.path_logo_png );
        plantillaContent = plantillaContent.Replace( "$str_nombre_beneficiario$", orden.str_nombre_beneficiario );
        plantillaContent = plantillaContent.Replace( "$int_codigo_opi$", orden.int_codigo_opi.ToString() );
        plantillaContent = plantillaContent.Replace( "$str_nombre_beneficiario$", orden.str_nombre_beneficiario );
        plantillaContent = plantillaContent.Replace( "$str_nombre_comercial_ord$", orden.str_nombre_comercial_ord );
        plantillaContent = plantillaContent.Replace( "$str_ident_beneficiario$", orden.str_ident_beneficiario );
        plantillaContent = plantillaContent.Replace( "$str_nombre_destino$", orden.str_nombre_destino );
        plantillaContent = plantillaContent.Replace( "$str_cta_beneficiario$", orden.str_cta_beneficiario );
        plantillaContent = plantillaContent.Replace( "$str_tipo_cta_beneficiario$", orden.str_tipo_cta_beneficiario );
        plantillaContent = plantillaContent.Replace( "$str_nombre_cta_beneficiario$", orden.str_nombre_cta_beneficiario );
        plantillaContent = plantillaContent.Replace( "$str_id_cta_beneficiario$", orden.str_id_cta_beneficiario );
        plantillaContent = plantillaContent.Replace( "$dtt_fecha_ingresa$", orden.dtt_fecha_ingresa );
        plantillaContent = plantillaContent.Replace( "$dec_monto$", orden.dec_monto.ToString( formatMoney ) );
        plantillaContent = plantillaContent.Replace( "$str_tipo_comprobante$", orden.str_tipo_comprobante );
        plantillaContent = plantillaContent.Replace( "$str_num_combrobante$", orden.str_num_combrobante );
        plantillaContent = plantillaContent.Replace( "$str_observaciones$", orden.str_observaciones );
        plantillaContent = plantillaContent.Replace( "$str_oficina_origen$", orden.str_oficina_origen );
        plantillaContent = plantillaContent.Replace( "$str_tramitado$", orden.str_tramitado );
        plantillaContent = plantillaContent.Replace( "$dec_valor_factura$", orden.dec_valor_factura.ToString( formatMoney ) );
        plantillaContent = plantillaContent.Replace( "$str_num_retencion$", orden.str_num_retencion );
        plantillaContent = plantillaContent.Replace( "$str_planilla_contable$", orden.str_planilla_contable );
        plantillaContent = plantillaContent.Replace( "$str_solicitud_pago$", orden.str_solicitud_pago );
        plantillaContent = plantillaContent.Replace( "$str_info_adicional$", orden.str_info_adicional );
        plantillaContent = plantillaContent.Replace( "$str_usuario_ingresa$", orden.str_usuario_ingresa );
        plantillaContent = plantillaContent.Replace( "$str_usuario_revisa$", orden.str_usuario_revisa );
        plantillaContent = plantillaContent.Replace( "$str_usuario_aprueba$", orden.str_usuario_aprueba );
        
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
    public static string GenerarInterbancaria(DetalleOpi orden, ApiConfig apiConfig)
    {
        const string formatMoney = "0.00";
        var pathPlantilla = $"{Directory.GetCurrentDirectory()}/{apiConfig.path_template_autorizacion_interbancaria}";
        var plantillaContent = File.ReadAllText( pathPlantilla );
        plantillaContent = plantillaContent.Replace( "$str_logo$", apiConfig.path_logo_png );
        plantillaContent = plantillaContent.Replace( "$int_codigo_opi$", orden.int_codigo_opi.ToString() );
        plantillaContent = plantillaContent.Replace( "$str_nombre_beneficiario$", orden.str_nombre_beneficiario );
        plantillaContent = plantillaContent.Replace( "$dtt_fecha_ingresa$", orden.dtt_fecha_ingresa );
        plantillaContent = plantillaContent.Replace( "$str_nombre_ordenante$", orden.str_nombre_ordenante );
        plantillaContent = plantillaContent.Replace( "$str_ident_ordenante$", orden.str_ident_ordenante );
        plantillaContent = plantillaContent.Replace( "$str_cta_ordenante$", orden.str_cta_ordenante );
        plantillaContent = plantillaContent.Replace( "$str_tipo_cta_ordenante$", orden.str_tipo_cta_ordenante );
        plantillaContent = plantillaContent.Replace( "$str_nombre_beneficiario$", orden.str_nombre_beneficiario );
        plantillaContent = plantillaContent.Replace( "$str_ident_beneficiario$", orden.str_ident_beneficiario );
        plantillaContent = plantillaContent.Replace( "$str_nombre_destino$", orden.str_nombre_destino );
        plantillaContent = plantillaContent.Replace( "$str_cta_beneficiario$", orden.str_cta_beneficiario );
        plantillaContent = plantillaContent.Replace( "$str_tipo_cta_beneficiario$", orden.str_tipo_cta_beneficiario );
        plantillaContent = plantillaContent.Replace( "$dec_monto$", orden.dec_monto.ToString( formatMoney ) );
        plantillaContent = plantillaContent.Replace( "$str_observaciones$", orden.str_observaciones );
        plantillaContent = plantillaContent.Replace( "$str_usuario_ingresa$", orden.str_usuario_ingresa );
        plantillaContent = plantillaContent.Replace( "$str_usuario_revisa$", orden.str_usuario_revisa );
        plantillaContent = plantillaContent.Replace( "$str_usuario_aprueba$", orden.str_usuario_aprueba );
        
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