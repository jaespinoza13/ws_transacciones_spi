using Application.Common.Models;
using Domain.Entities.Opis;
using iText.Html2pdf;
using iText.Kernel.Pdf;

namespace Application.Features.Opis.Queries.Cuadre.Common;

public static class CuadreOpis
{
    public static string GenerarCuadreOpis(ReqCuadreOpis request, IReadOnlyList<OrdenPago> list, ApiConfig apiConfig)
    {
        var pathPlantilla = $"{Directory.GetCurrentDirectory()}/{apiConfig.path_template_cuadre_opis}";
        var pathLogo = $"{Directory.GetCurrentDirectory()}/{apiConfig.path_logo_png}";
        
        var total = list.Sum( x => Convert.ToDecimal( x.dec_monto )).ToString( "N2" );
        
        var plantillaContent = File.ReadAllText( pathPlantilla );
        plantillaContent = plantillaContent.Replace( "$str_logo$", pathLogo );
        plantillaContent = plantillaContent.Replace( "$dtt_fecha$", request.dtt_fecha.ToString( "dd/MM/yyyy" ) );
        plantillaContent = plantillaContent.Replace( "$int_total_opis$", list.Count.ToString() );
        plantillaContent = plantillaContent.Replace( "$int_monto_total_opis$", total );
        plantillaContent = plantillaContent.Replace( "$str_login$", request.str_login );
        
        var valores = list.Aggregate( string.Empty, (current, opi) => current + "<tr>" + $"<td>{opi.int_codigo_opi.ToString()}</td>" + $"<td>{opi.str_cuenta_ordenante}</td>" + $"<td>{opi.str_nombre_ordenante}</td>" + $"<td>{opi.str_cuenta_beneficiario}</td>" + $"<td>{opi.str_nombre_beneficiario}</td>" + $"<td>{opi.str_banco_destino}</td>" + $"<td>{opi.str_tipo_transaccion}</td>" + $"<td>{opi.str_estado_interno}</td>" + $"<td>{opi.dec_monto}</td>" + "</tr>" );
        
        plantillaContent = plantillaContent.Replace( "$str_tabla_opis$", valores );
        
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