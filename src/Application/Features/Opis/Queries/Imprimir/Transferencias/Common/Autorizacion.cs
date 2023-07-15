using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Style = iText.Layout.Style;
using iText.Kernel.Colors;
using Table = iText.Layout.Element.Table;
using Image = iText.Layout.Element.Image;
using Application.Common.Models;
using Domain.Entities.Opis;

namespace Application.Features.Opis.Queries.Imprimir.Transferencias.Common;

public static class Autorizacion
{
    public static byte[] GenerarAutorizacionPersonaNatural(AutorizacionTransfExterna autorizacion, ApiSettings settings)
    {
        const string title = "SOLICITUD Y AUTORIZACIÓN DE DÉBITO POR TRANSFERENCIAS DE FONDOS A OTRAS INSTITUCIONES FINANCIERAS";
        var ms = new MemoryStream();
        var pw = new PdfWriter( ms );
        var pdf = new PdfDocument( pw );
        var doc = new Document( pdf, PageSize.LETTER );
        doc.SetMargins( 30, 30, 30, 30 );
        var encabezado = EncabezadoAutorizacion( title, settings );
        doc.Add( encabezado );

        var headerContent = new Div();
        var paragraphContent = new Paragraph();
        headerContent.SetMarginTop( 5 );
        headerContent.SetMarginBottom( 10 );
        paragraphContent.Add( new Text( autorizacion.str_mensaje ).AddStyle( GetStyleReportes()["styleContent"] ) );
        headerContent.Add( paragraphContent ).SetTextAlignment( TextAlignment.JUSTIFIED );
        doc.Add( headerContent );

        var tableDetalleTransferencia = DetalleTransferencia( autorizacion );
        doc.Add( tableDetalleTransferencia );

        var bodyContent = new Div();
        var paragraphBodyContent = new Paragraph();
        bodyContent.SetMarginTop( 5 );
        bodyContent.SetMarginBottom( 5 );
        paragraphBodyContent.Add(
            new Text( autorizacion.str_concepto ).AddStyle( GetStyleReportes()["styleContent"] ) );
        bodyContent.Add( paragraphBodyContent ).SetTextAlignment( TextAlignment.JUSTIFIED );
        doc.Add( bodyContent );

        const string body2 =
            "Conocedor de las penas de perjurio y de las disposiciones contenidas en la Ley relacionada con la prevención de Lavado de Activos, declaro bajo juramento que los fondos que transfiero desde Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda.; no serán destinados a la realización o financiamiento de ninguna actividad ilícita. Autorizo expresamente a Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda.; para que realice los análisis y verificaciones que considere necesarios; así como, para que informe a las autoridades competentes en caso de llegar a determinar que en mi cuenta de ahorros antes singularizada se han registrado/generado operaciones y/o transacciones inusuales e injustificadas.";
        var contentBody2 = new Paragraph(
            body2
        ).AddStyle( GetStyleReportes()["styleContent"] );
        doc.Add( contentBody2 );

        const string body3 =
            "Las presentes solicitud y autorizaciones las realizo bajo mi absoluta responsabilidad, por lo que una vez que las mismas sean procesadas por el personal autorizado de la Cooperativa, nada tengo que reclamar al respecto ni a la Cooperativa ni a su personal.";
        var contentBody3 = new Paragraph(
            body3
        ).AddStyle( GetStyleReportes()["styleContent"] );
        doc.Add( contentBody3 );

        const string body4 =
            "Declaro que he leído, entiendo y me encuentro de acuerdo con los datos/información consignados en el presente documento, razón por la que renuncio a presentar cualquier tipo de reclamación, acción judicial o extrajudicial, civil/penal/administrativa, en contra de Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda.; o de sus trabajadores.";
        var contentBody4 = new Div();
        var paragraphContentBody4 = new Paragraph();
        paragraphContentBody4.Add( new Text( "DECLARACIÓN: " ).AddStyle( GetStyleReportes()["styleContentBold"] ) );
        paragraphContentBody4
            .Add( new Text( body4 ) )
            .AddStyle( GetStyleReportes()["styleContent"] );
        contentBody4.Add( paragraphContentBody4 ).SetTextAlignment( TextAlignment.JUSTIFIED );
        doc.Add( contentBody4 );

        var withFirmas = new[] { 50f, 50f };
        var tableFirmas = new Table( UnitValue.CreatePercentArray( withFirmas ) ).UseAllAvailableWidth();
        tableFirmas.SetHorizontalAlignment( HorizontalAlignment.CENTER );
        tableFirmas.SetVerticalAlignment( VerticalAlignment.MIDDLE );
        tableFirmas.SetMarginTop( 30 );

        var styeCellFirmas = new Style()
            .SetBorder( Border.NO_BORDER )
            .SetHeight( 12f )
            .SetTextAlignment( TextAlignment.CENTER );
        var cellFirmas = new Cell()
            .Add( new Paragraph( "_____________________________________" ).AddStyle(
                GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( cellFirmas );
        cellFirmas = new Cell()
            .Add( new Paragraph( "_____________________________________" ).AddStyle(
                GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( cellFirmas );
        var representante = new Cell()
            .Add( new Paragraph( autorizacion.str_representante + ": " ).AddStyle(
                GetStyleReportes()["styleFirmasContent"] ) )
            .AddStyle( styeCellFirmas );
        tableFirmas.AddCell( representante );
        var genera = new Cell()
            .Add( new Paragraph( "RECIBIDO POR:" ).AddStyle( GetStyleReportes()["styleFirmasContent"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( genera );
        var nombreRepresentante = new Cell()
            .Add(
                new Paragraph( autorizacion.str_nombre_ord ).AddStyle( GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );
        tableFirmas.AddCell( nombreRepresentante );
        var nombreGenera = new Cell()
            .Add( new Paragraph( autorizacion.str_usuario ).AddStyle(
                GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( nombreGenera );
        var espacio = new Cell().Add( new Paragraph( "" ).AddStyle( GetStyleReportes()["styleFirmasContent"] ) )
            .AddStyle( styeCellFirmas );
        tableFirmas.AddCell( espacio );
        var oficinaGenera = new Cell()
            .Add(
                new Paragraph( autorizacion.str_oficina ).AddStyle( GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( oficinaGenera );
        doc.Add( tableFirmas );

        var contentFooter = FooterAutorizacion();
        doc.Add( contentFooter );

        doc.Close();
        var bytes = ms.ToArray();
        ms = new MemoryStream();
        ms.Write( bytes, 0, bytes.Length );
        ms.Position = 0;
        return bytes;
    }

    public static byte[] GenerarAutorizacionPersonaJuridica(AutorizacionTransfExterna autorizacion,
        ApiSettings settings)
    {
        const string title = "SOLICITUD Y AUTORIZACIÓN DE TRANSFERENCIAS DE FONDOS A OTRAS INSTITUCIONES FINANCIERAS";
        var ms = new MemoryStream();
        var pw = new PdfWriter( ms );
        var pdf = new PdfDocument( pw );
        var doc = new Document( pdf, PageSize.LETTER );
        doc.SetMargins( 30, 30, 30, 30 );
        var encabezado = EncabezadoAutorizacion( title, settings );
        doc.Add( encabezado );

        var headerContent = new Div();
        var paragraphContent = new Paragraph();
        headerContent.SetMarginTop( 5 );
        headerContent.SetMarginBottom( 10 );
        paragraphContent.Add( new Text( autorizacion.str_mensaje ).AddStyle( GetStyleReportes()["styleContent"] ) );
        headerContent.Add( paragraphContent ).SetTextAlignment( TextAlignment.JUSTIFIED );
        doc.Add( headerContent );

        var tableDetalleTransferencia = DetalleTransferencia( autorizacion );
        doc.Add( tableDetalleTransferencia );

        var bodyContent = new Div();
        var paragraphBodyContent = new Paragraph();
        bodyContent.SetMarginTop( 5 );
        bodyContent.SetMarginBottom( 5 );
        paragraphBodyContent.Add(
            new Text( autorizacion.str_concepto ).AddStyle( GetStyleReportes()["styleContent"] ) );
        bodyContent.Add( paragraphBodyContent ).SetTextAlignment( TextAlignment.JUSTIFIED );
        doc.Add( bodyContent );

        const string body2 =
            "Conocedor (es) de las penas de perjurio y de las disposiciones contenidas en la Ley para reprimir el Lavado de Activos, declaro (amos) bajo juramento que los fondos que transfiero o transferimos desde la Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda.; no serán destinados a la realización o financiamiento de ninguna actividad ilícita. Autorizo (amos) expresamente a la Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda.; a realizar los análisis y verificaciones que considere necesarios; así como de informar a las autoridades competentes en caso de llegar a determinar la existencia de operaciones y/o transacciones inusuales e injustificadas. En virtud de lo autorizado, renuncio (amos) a instaurar por este motivo cualquier tipo de acción civil, penal o administrativa en contra de la Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda.";
        var contentBody2 = new Paragraph(
            body2
        ).AddStyle( GetStyleReportes()["styleContent"] );
        doc.Add( contentBody2 );

        var withFirmas = new[] { 50f, 50f };
        var tableFirmas = new Table( UnitValue.CreatePercentArray( withFirmas ) ).UseAllAvailableWidth();
        tableFirmas.SetHorizontalAlignment( HorizontalAlignment.CENTER );
        tableFirmas.SetVerticalAlignment( VerticalAlignment.MIDDLE );
        tableFirmas.SetMarginTop( 30 );

        var styeCellFirmas = new Style()
            .SetBorder( Border.NO_BORDER )
            .SetHeight( 12f )
            .SetTextAlignment( TextAlignment.CENTER );
        var cellFirmas = new Cell()
            .Add( new Paragraph( "_____________________________________" ).AddStyle(
                GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( cellFirmas );
        cellFirmas = new Cell()
            .Add( new Paragraph( "_____________________________________" ).AddStyle(
                GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( cellFirmas );
        var representante = new Cell()
            .Add( new Paragraph( autorizacion.str_representante + ": " ).AddStyle(
                GetStyleReportes()["styleFirmasContent"] ) )
            .AddStyle( styeCellFirmas );
        tableFirmas.AddCell( representante );
        var genera = new Cell()
            .Add( new Paragraph( "RECIBIDO POR:" ).AddStyle( GetStyleReportes()["styleFirmasContent"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( genera );
        var nombreRepresentante = new Cell()
            .Add(
                new Paragraph( autorizacion.str_nombre_ord ).AddStyle( GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );
        tableFirmas.AddCell( nombreRepresentante );
        var nombreGenera = new Cell()
            .Add( new Paragraph( autorizacion.str_usuario ).AddStyle(
                GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( nombreGenera );
        var espacio = new Cell().Add( new Paragraph( "" ).AddStyle( GetStyleReportes()["styleFirmasContent"] ) )
            .AddStyle( styeCellFirmas );
        tableFirmas.AddCell( espacio );
        var oficinaGenera = new Cell()
            .Add(
                new Paragraph( autorizacion.str_oficina ).AddStyle( GetStyleReportes()["styleFirmasContentBold"] ) )
            .AddStyle( styeCellFirmas );

        tableFirmas.AddCell( oficinaGenera );
        doc.Add( tableFirmas );

        var contentFooter = FooterAutorizacion();
        doc.Add( contentFooter );

        doc.Close();
        var bytes = ms.ToArray();
        ms = new MemoryStream();
        ms.Write( bytes, 0, bytes.Length );
        ms.Position = 0;

        return bytes;
    }

    private static Table EncabezadoAutorizacion(string title, ApiSettings settings)
    {
        var logoPath = settings.path_logo_png;
        var logoData = ImageDataFactory.Create( logoPath );
        var cellWidthHeader = new[] { 30f, 70f };
        var tableHeader = new Table( UnitValue.CreatePercentArray( cellWidthHeader ) ).UseAllAvailableWidth();
        tableHeader.SetBorder( Border.NO_BORDER );
        tableHeader.SetHorizontalAlignment( HorizontalAlignment.CENTER );
        tableHeader.SetVerticalAlignment( VerticalAlignment.MIDDLE );

        var cellHeader = new Cell().Add( new Image( logoData ).SetAutoScale( true ) ).SetBorder( Border.NO_BORDER );
        tableHeader.AddCell( cellHeader );
        cellHeader =
            new Cell()
                .Add( new Paragraph( title ).AddStyle( GetStyleReportes()["titleHeaderStyle"] )
                    .AddStyle( GetStyleReportes()["subtitleHeaderStyle"] )
                ).SetBorder( Border.NO_BORDER );
        tableHeader.AddCell( cellHeader );
        return tableHeader;
    }

    private static Table DetalleTransferencia(AutorizacionTransfExterna autorizacion)
    {
        var cellWidthDetalle = new[] { 30f, 70f };
        var tableDetalle = new Table( UnitValue.CreatePercentArray( cellWidthDetalle ) ).UseAllAvailableWidth();
        tableDetalle.SetBorder( Border.NO_BORDER );
        tableDetalle.SetMarginBottom( 5 );
        tableDetalle.SetHorizontalAlignment( HorizontalAlignment.CENTER );
        tableDetalle.SetVerticalAlignment( VerticalAlignment.MIDDLE );

        var styleDetalleCell = new Style()
            .SetFontSize( 8 )
            .SetTextAlignment( TextAlignment.JUSTIFIED )
            .SetBorder( Border.NO_BORDER );

        var cellDetalle = new Cell()
            .Add(
                new Paragraph( "Fecha/Hora de Impresión: " ).AddStyle( GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( DateTime.Now.ToString( "dd/MM/yyyy HH:mm:ss" ) ).AddStyle(
                GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( "Número de Referencia: " ).AddStyle( GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( autorizacion.int_codigo_opi.ToString() ).AddStyle(
                GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( "Cuenta Débito: " ).AddStyle(
                GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add(
                new Paragraph( autorizacion.str_num_cuenta_ord ).AddStyle( GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( "Cuenta Crédito: " ).AddStyle(
                GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add(
                new Paragraph( autorizacion.str_num_cuenta_benef ).AddStyle( GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );

        cellDetalle = new Cell()
            .Add( new Paragraph( "Tipo de Cta. Crédito: " ).AddStyle(
                GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( autorizacion.str_tipo_cuenta_benef ).AddStyle( GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );

        cellDetalle = new Cell()
            .Add( new Paragraph( "Institución: " ).AddStyle(
                GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( autorizacion.str_banco_benef ).AddStyle(
                GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( "Nombre Cta. Crédito: " ).AddStyle(
                GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( autorizacion.str_razon_social_benef ).AddStyle(
                GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( "Identificación Cta. Crédito: " ).AddStyle(
                GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( autorizacion.str_cedula_ruc_benef ).AddStyle(
                GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );

        cellDetalle = new Cell()
            .Add( new Paragraph( "Valor a Transferir: " ).AddStyle( GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( "$ " + autorizacion.str_monto_opi ).AddStyle( GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );

        cellDetalle = new Cell()
            .Add(
                new Paragraph( "Motivo de la Transferencia: " ).AddStyle( GetStyleReportes()["styleContentBold"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );
        cellDetalle = new Cell()
            .Add( new Paragraph( autorizacion.str_observaciones ).AddStyle( GetStyleReportes()["styleContent"] ) )
            .AddStyle( styleDetalleCell );
        tableDetalle.AddCell( cellDetalle );

        return tableDetalle;
    }

    private static Div FooterAutorizacion()
    {
        var paragraphContentFooter = new Paragraph();
        var paragraphContentFooter2 = new Paragraph();
        var contentFooter = new Div();
        paragraphContentFooter.Add(
            new Text( "Estimado Socio/Cliente: " ).AddStyle( GetStyleReportes()["styleContentBold"] ) );
        paragraphContentFooter
            .Add( new Text(
                "Le informamos que la presente solicitud está sujeta a la revisión y aprobación por parte del personal de la Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda.; en caso de encontrar novedades, estas serán comunicadas inmediatamente a su número de teléfono celular y/o correo electrónico registrados en la Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda., y por su seguridad esta transferencia no se procesará." ) )
            .AddStyle( GetStyleReportes()["styleContent"] );

        contentFooter.Add( paragraphContentFooter ).SetTextAlignment( TextAlignment.JUSTIFIED );

        paragraphContentFooter2.Add(
            new Text(
                    "Si usted presentó la solicitud en nuestras oficinas hasta las 16:00, los fondos serán acreditados a la cuenta que usted solicitó en el transcurso del día, posterior a la aprobación por parte del personal de la Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda.; caso contrario se acreditarán los fondos en la cuenta destino a partir del siguiente día hábil." )
                .AddStyle( GetStyleReportes()["styleContent"] ) );
        contentFooter.Add( paragraphContentFooter2 ).SetTextAlignment( TextAlignment.JUSTIFIED );
        return contentFooter;
    }

    private static Dictionary<string, Style> GetStyleReportes()
    {
        var stylesAutorizacion = new Dictionary<string, Style>();

        var titleHeaderStyle = new Style()
            .SetFontSize( 10 )
            .SetBold()
            .SetMarginTop( 4 )
            .SetTextAlignment( TextAlignment.CENTER );

        var subtitleHeaderStyle = new Style()
            .SetFontSize( 9 )
            .SetBold()
            .SetTextAlignment( TextAlignment.CENTER )
            .SetMarginLeft( 10f )
            .SetMarginRight( 10f );

        var styleContent = new Style()
            .SetFontSize( 8 )
            .SetTextAlignment( TextAlignment.JUSTIFIED )
            .SetBorder( Border.NO_BORDER );

        var styleContentBold = new Style()
            .SetFontSize( 8 )
            .SetBold()
            .SetTextAlignment( TextAlignment.JUSTIFIED )
            .SetBorder( Border.NO_BORDER );

        var styleFirmasContent = new Style()
            .SetFontSize( 7 )
            .SetTextAlignment( TextAlignment.CENTER );

        var styleFirmasContentBold = new Style()
            .SetFontSize( 7 )
            .SetBold()
            .SetTextAlignment( TextAlignment.CENTER );

        var styeTitleCuadre = new Style()
            .SetFontSize( 11 )
            .SetBold()
            .SetTextAlignment( TextAlignment.CENTER )
            .SetMarginTop( 10f )
            .SetBorder( Border.NO_BORDER );

        var styleSubTitleCuadre = new Style()
            .SetFontSize( 9 )
            .SetBold()
            .SetTextAlignment( TextAlignment.CENTER )
            .SetBorder( Border.NO_BORDER );

        var styleSubTitle2Cuadre = new Style()
            .SetFontSize( 9 )
            .SetBold()
            .SetItalic()
            .SetTextAlignment( TextAlignment.CENTER )
            .SetBorder( Border.NO_BORDER );

        var styleCellCuadreBody = new Style()
            .SetBackgroundColor( ColorConstants.LIGHT_GRAY )
            .SetTextAlignment( TextAlignment.CENTER );

        var styleCellCuadreBodyValue = new Style()
            .SetTextAlignment( TextAlignment.CENTER );

        var styleTitleCuadreCellBody = new Style()
            .SetFontSize( 7 )
            .SetTextAlignment( TextAlignment.CENTER )
            .SetBold();

        var styleValueCuadreCellBody = new Style()
            .SetFontSize( 7 )
            .SetTextAlignment( TextAlignment.CENTER );

        var styleFirmaValue = new Style()
            .SetTextAlignment( TextAlignment.CENTER )
            .SetFontSize( 6 );

        var styleCellFirma = new Style()
            .SetBorder( Border.NO_BORDER )
            .SetTextAlignment( TextAlignment.CENTER );

        var styleTitleDetalleCuadre = new Style()
            .SetFontSize( 8 )
            .SetTextAlignment( TextAlignment.LEFT )
            .SetBold()
            .SetMarginTop( 10f );

        var styleOrdenTitle = new Style()
            .SetFontSize( 9 )
            .SetTextAlignment( TextAlignment.CENTER )
            .SetBold();

        var styleOrdenSubTitle = new Style()
            .SetFontSize( 8 )
            .SetTextAlignment( TextAlignment.LEFT )
            .SetBold();

        var styleOrdenValue = new Style()
            .SetFontSize( 8 )
            .SetTextAlignment( TextAlignment.RIGHT );

        var styleOrdenDetalle = new Style()
            .SetFontSize( 8 )
            .SetTextAlignment( TextAlignment.JUSTIFIED );

        var styleFirmasOrdenValue = new Style()
            .SetFontSize( 7 )
            .SetTextAlignment( TextAlignment.CENTER );

        var styleFirmasOrdenTitle = new Style()
            .SetFontSize( 7 )
            .SetBold()
            .SetTextAlignment( TextAlignment.CENTER );


        stylesAutorizacion.Add( "titleHeaderStyle", titleHeaderStyle );
        stylesAutorizacion.Add( "subtitleHeaderStyle", subtitleHeaderStyle );
        stylesAutorizacion.Add( "styleContent", styleContent );
        stylesAutorizacion.Add( "styleContentBold", styleContentBold );
        stylesAutorizacion.Add( "styleFirmasContent", styleFirmasContent );
        stylesAutorizacion.Add( "styleFirmasContentBold", styleFirmasContentBold );
        stylesAutorizacion.Add( "styeTitleCuadre", styeTitleCuadre );
        stylesAutorizacion.Add( "styleSubTitleCuadre", styleSubTitleCuadre );
        stylesAutorizacion.Add( "styleSubTitle2Cuadre", styleSubTitle2Cuadre );
        stylesAutorizacion.Add( "styleCellCuadreBody", styleCellCuadreBody );
        stylesAutorizacion.Add( "styleCellCuadreBodyValue", styleCellCuadreBodyValue );
        stylesAutorizacion.Add( "styleTitleCuadreCellBody", styleTitleCuadreCellBody );
        stylesAutorizacion.Add( "styleValueCuadreCellBody", styleValueCuadreCellBody );
        stylesAutorizacion.Add( "styleFirmaValue", styleFirmaValue );
        stylesAutorizacion.Add( "styleCellFirma", styleCellFirma );
        stylesAutorizacion.Add( "styleTitleDetalleCuadre", styleTitleDetalleCuadre );
        stylesAutorizacion.Add( "styleOrdenTitle", styleOrdenTitle );
        stylesAutorizacion.Add( "styleOrdenSubTitle", styleOrdenSubTitle );
        stylesAutorizacion.Add( "styleOrdenValue", styleOrdenValue );
        stylesAutorizacion.Add( "styleOrdenDetalle", styleOrdenDetalle );
        stylesAutorizacion.Add( "styleFirmasOrdenValue", styleFirmasOrdenValue );
        stylesAutorizacion.Add( "styleFirmasOrdenTitle", styleFirmasOrdenTitle );


        return stylesAutorizacion;
    }
}