using Application.Common.Models;
using Domain.Entities.Opis;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Application.Features.Opis.Queries.Imprimir.OrdenPago.Common;

public static class Autorizacion
{
    public static byte[] GenerarInterbancaria(DetalleOpi orden, ApiConfig settings)
    {
        const string title =
            "Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda. \n\n AUTORIZACIÓN DE TRANSFERENCIA INTERBANCARIA";
        var ms = new MemoryStream();
        var pw = new PdfWriter(ms);
        var pdf = new PdfDocument(pw);
        var doc = new Document(pdf, PageSize.LETTER);
        doc.SetMargins(30, 30, 30, 30);
        var encabezado = EncabezadoAutorizacion(title, settings);
        doc.Add(encabezado);
        var styleCellOrdenTitle = new Style()
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(Border.NO_BORDER);

        var styleCellOrdenValue = new Style()
            .SetBorder(Border.NO_BORDER)
            .SetMarginTop(10)
            .SetMarginBottom(10);

        var cellWidth = new[] { 40f, 60f };

        var tableDetalle = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();
        tableDetalle.SetHorizontalAlignment(HorizontalAlignment.CENTER);
        tableDetalle.SetVerticalAlignment(VerticalAlignment.MIDDLE);
        tableDetalle.SetMarginTop(10);
        var cellOrden = new Cell()
            .Add(new Paragraph("NÚMERO DE ORDEN: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellOrden);
        cellOrden = new Cell()
            .Add(new Paragraph(orden.int_codigo_opi.ToString()).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellOrden);
        cellOrden = new Cell()
            .Add(new Paragraph("FECHA DE INGRESO: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellOrden);
        cellOrden = new Cell()
            .Add(new Paragraph(orden.dtt_fecha_ingresa).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellOrden);

        var cellTitleOrdenante = new Cell(2, 2)
            .Add(new Paragraph("DATOS DEL ORDENANTE").AddStyle(GetStyleReportes()["styleOrdenTitle"]))
            .AddStyle(styleCellOrdenTitle);
        tableDetalle.AddCell(cellTitleOrdenante);

        var cellDetalleOrdenante = new Cell()
            .Add(new Paragraph("NOMBRE DEL ORDENANTE: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleOrdenante);
        cellDetalleOrdenante = new Cell()
            .Add(new Paragraph(orden.str_nombre_ordenante).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleOrdenante);

        cellDetalleOrdenante = new Cell()
            .Add(
                new Paragraph("NÚMERO DE RUC DEL ORDENANTE: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleOrdenante);
        cellDetalleOrdenante = new Cell()
            .Add(new Paragraph(orden.str_ident_ordenante).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleOrdenante);

        cellDetalleOrdenante = new Cell()
            .Add(new Paragraph("NÚMERO DE CUENTA DEL ORDENANTE: ").AddStyle(
                GetStyleReportes()["styleOrdenSubTitle"])).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleOrdenante);
        cellDetalleOrdenante = new Cell()
            .Add(new Paragraph(orden.str_cta_ordenante).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleOrdenante);

        cellDetalleOrdenante = new Cell()
            .Add(
                new Paragraph("TIPO DE CUENTA DEL ORDENANTE: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleOrdenante);
        cellDetalleOrdenante = new Cell()
            .Add(new Paragraph(orden.str_tipo_cta_ordenante).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleOrdenante);

        var cellTitleBeneficiario = new Cell(2, 2)
            .Add(new Paragraph("DATOS DEL BENEFICIARIO").AddStyle(GetStyleReportes()["styleOrdenTitle"]))
            .AddStyle(styleCellOrdenTitle);
        tableDetalle.AddCell(cellTitleBeneficiario);

        var cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph("NOMBRE DEL BENEFICIARIO: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);
        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph(orden.str_nombre_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);

        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph("NÚMERO DE CÉDULA/RUC BENEFICIARIO: ").AddStyle(
                GetStyleReportes()["styleOrdenSubTitle"])).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);
        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph(orden.str_ident_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);

        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph("INSTITUCIÓN FINANCIERA DESTINO: ").AddStyle(
                GetStyleReportes()["styleOrdenSubTitle"])).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);
        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph(orden.str_nombre_destino).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);

        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph("NÚMERO DE CUENTA DEL BENEFICIARIO: ").AddStyle(
                GetStyleReportes()["styleOrdenSubTitle"])).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);
        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph(orden.str_cta_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);

        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph("TIPO DE CUENTA DEL BENEFICIARIO: ").AddStyle(
                GetStyleReportes()["styleOrdenSubTitle"])).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);
        cellDetalleBeneficiario = new Cell()
            .Add(new Paragraph(orden.str_tipo_cta_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleBeneficiario);

        var cellTitleInfo = new Cell(2, 2)
            .Add(new Paragraph("INFORMACIÓN DE LA TRANSFERENCIA").AddStyle(GetStyleReportes()["styleOrdenTitle"]))
            .AddStyle(styleCellOrdenTitle);
        tableDetalle.AddCell(cellTitleInfo);

        var cellDetalleTransferencia = new Cell()
            .Add(new Paragraph("VALOR A TRANSFERIR: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleTransferencia);
        cellDetalleTransferencia = new Cell()
            .Add(new Paragraph("$ " + orden.dec_monto).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleTransferencia);
        cellDetalleTransferencia = new Cell()
            .Add(new Paragraph("MOTIVO DE LA TRANSFERENCIA: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleTransferencia);
        cellDetalleTransferencia = new Cell()
            .Add(new Paragraph(orden.str_observaciones).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleTransferencia);

        var cellTitleFirmas = new Cell(2, 2)
            .Add(new Paragraph("AUTORIZACIÓN").AddStyle(GetStyleReportes()["styleOrdenTitle"]))
            .AddStyle(styleCellOrdenTitle).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellTitleFirmas);
        doc.Add(tableDetalle);

        var tableFirmas =
            FirmantesOrdenPago(orden.str_usuario_ingresa, orden.str_usuario_revisa, orden.str_usuario_aprueba);
        doc.Add(tableFirmas);
        doc.Close();
        var bytes = ms.ToArray();
        ms = new MemoryStream();
        ms.Write(bytes, 0, bytes.Length);
        ms.Position = 0;
        return bytes;
    }

    public static byte[] GenerarOrdenPago(DetalleOpi orden, ApiConfig settings)
    {
        const string title =
            "Cooperativa de Ahorro y Crédito Vicentina “Manuel Esteban Godoy Ortega” Ltda. \n\n AUTORIZACIÓN DE TRANSFERENCIA";
        var ms = new MemoryStream();
        var pw = new PdfWriter(ms);
        var pdf = new PdfDocument(pw);
        var doc = new Document(pdf, PageSize.LETTER);
        doc.SetMargins(30, 30, 30, 30);
        var encabezado = EncabezadoAutorizacion(title, settings);
        doc.Add(encabezado);
        var styleCellOrdenTitle = new Style()
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(Border.NO_BORDER);

        var styleCellOrdenValue = new Style()
            .SetBorder(Border.NO_BORDER);

        var cellWidth = new[] { 40f, 60f };

        var tableDetalle = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();
        tableDetalle.SetHorizontalAlignment(HorizontalAlignment.CENTER);
        tableDetalle.SetVerticalAlignment(VerticalAlignment.MIDDLE);
        tableDetalle.SetMarginTop(10);
        var cellOrden = new Cell()
            .Add(new Paragraph("NÚMERO DE ORDEN: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellOrden);
        cellOrden = new Cell()
            .Add(new Paragraph(orden.int_codigo_opi.ToString()).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellOrden);

        var cellTitleProveedor = new Cell(2, 2)
            .Add(new Paragraph("DATOS DEL PROVEEDOR").AddStyle(GetStyleReportes()["styleOrdenTitle"]))
            .AddStyle(styleCellOrdenTitle);
        tableDetalle.AddCell(cellTitleProveedor);

        var cellDetalleProveedor = new Cell()
            .Add(new Paragraph("RAZÓN SOCIAL: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);
        cellDetalleProveedor = new Cell()
            .Add(new Paragraph(orden.str_nombre_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);

        cellDetalleProveedor = new Cell()
            .Add(new Paragraph("NOMBRE COMERCIAL: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);
        cellDetalleProveedor = new Cell()
            .Add(new Paragraph(orden.str_nombre_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);

        cellDetalleProveedor = new Cell()
            .Add(new Paragraph("RUC: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);
        cellDetalleProveedor = new Cell()
            .Add(new Paragraph(orden.str_ident_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);

        cellDetalleProveedor = new Cell()
            .Add(new Paragraph("INSTITUCIÓN FINANCIERA: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);
        cellDetalleProveedor = new Cell()
            .Add(new Paragraph(orden.str_nombre_destino).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);

        cellDetalleProveedor = new Cell()
            .Add(new Paragraph("NÚMERO DE CUENTA: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);
        cellDetalleProveedor = new Cell()
            .Add(new Paragraph(orden.str_cta_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);

        cellDetalleProveedor = new Cell()
            .Add(new Paragraph("TIPO DE CUENTA: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);
        cellDetalleProveedor = new Cell()
            .Add(new Paragraph(orden.str_tipo_cta_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);

        cellDetalleProveedor = new Cell()
            .Add(new Paragraph("TITULAR DE LA CUENTA: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);
        cellDetalleProveedor = new Cell()
            .Add(new Paragraph(orden.str_nombre_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);

        cellDetalleProveedor = new Cell()
            .Add(new Paragraph("IDENTIFICACIÓN TITULAR DE LA CUENTA: ").AddStyle(
                GetStyleReportes()["styleOrdenSubTitle"])).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);
        cellDetalleProveedor = new Cell()
            .Add(new Paragraph(orden.str_ident_beneficiario).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDetalleProveedor);

        var cellTitleDatosPago = new Cell(2, 2)
            .Add(new Paragraph("DATOS DEL PAGO").AddStyle(GetStyleReportes()["styleOrdenTitle"]))
            .AddStyle(styleCellOrdenTitle).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellTitleDatosPago);

        var cellDatosPago = new Cell()
            .Add(new Paragraph("FECHA DE INGRESO: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(new Paragraph(orden.dtt_fecha_ingresa).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(new Paragraph("MONTO A PAGAR: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(new Paragraph("$ " + orden.dec_monto).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(
                new Paragraph("TIPO DE COMPROBANTE DE VENTA: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(new Paragraph(orden.str_tipo_comprobante).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(new Paragraph("NÚMERO DE COMPROBANTE DE VENTA: ").AddStyle(
                GetStyleReportes()["styleOrdenSubTitle"])).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(new Paragraph(orden.str_num_combrobante).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(new Paragraph("CONCEPTO DE PAGO: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);
        cellDatosPago = new Cell()
            .Add(new Paragraph(orden.str_observaciones).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellDatosPago);

        var cellTitleInformacion = new Cell(2, 2)
            .Add(new Paragraph("INFORMACIÓN DEL PAGO").AddStyle(GetStyleReportes()["styleOrdenTitle"]))
            .AddStyle(styleCellOrdenTitle).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellTitleInformacion);

        var cellInformacionPago = new Cell()
            .Add(new Paragraph("OFICINA: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph(orden.str_oficina_origen).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);

        cellInformacionPago = new Cell()
            .Add(new Paragraph("TRAMITADO POR: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph(orden.str_tramitado).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph("VALOR DEL COMPROBANTE DE VENTA: ").AddStyle(
                GetStyleReportes()["styleOrdenSubTitle"])).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph("$ " + orden.dec_valor_factura).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(
                new Paragraph("N° COMPROBANTE DE RETENCIÓN: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph(orden.str_num_retencion).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph("N° COMPROBANTE CONTABLE: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph(orden.str_planilla_contable).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph("N° SOLICITUD DE PAGO: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph(orden.str_solicitud_pago).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph("OBSERVACIONES DEL PAGO: ").AddStyle(GetStyleReportes()["styleOrdenSubTitle"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        cellInformacionPago = new Cell()
            .Add(new Paragraph(orden.str_info_adicional).AddStyle(GetStyleReportes()["styleOrdenValue"]))
            .AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellInformacionPago);
        var cellTitleFirmas = new Cell(2, 2)
            .Add(new Paragraph("AUTORIZACIÓN").AddStyle(GetStyleReportes()["styleOrdenTitle"]))
            .AddStyle(styleCellOrdenTitle).AddStyle(styleCellOrdenValue);
        tableDetalle.AddCell(cellTitleFirmas);
        doc.Add(tableDetalle);
        var tableFirmas =
            FirmantesOrdenPago(orden.str_usuario_ingresa, orden.str_usuario_revisa, orden.str_usuario_aprueba);
        doc.Add(tableFirmas);
        doc.Close();
        var bytes = ms.ToArray();
        ms = new MemoryStream();
        ms.Write(bytes, 0, bytes.Length);
        ms.Position = 0;

        return bytes;
    }

    private static Table EncabezadoAutorizacion(string title, ApiConfig settings)
    {
        var logoPath = settings.path_logo_png;
        var logoData = ImageDataFactory.Create(logoPath);
        var cellWidthHeader = new[] { 30f, 70f };
        var tableHeader = new Table(UnitValue.CreatePercentArray(cellWidthHeader)).UseAllAvailableWidth();
        tableHeader.SetBorder(Border.NO_BORDER);
        tableHeader.SetHorizontalAlignment(HorizontalAlignment.CENTER);
        tableHeader.SetVerticalAlignment(VerticalAlignment.MIDDLE);

        var cellHeader = new Cell().Add(new Image(logoData).SetAutoScale(true)).SetBorder(Border.NO_BORDER);
        tableHeader.AddCell(cellHeader);
        cellHeader =
            new Cell()
                .Add(new Paragraph(title).AddStyle(GetStyleReportes()["titleHeaderStyle"])
                    .AddStyle(GetStyleReportes()["subtitleHeaderStyle"])
                ).SetBorder(Border.NO_BORDER);
        tableHeader.AddCell(cellHeader);
        return tableHeader;
    }

    private static Table FirmantesOrdenPago(string ingresa, string revisa, string autoriza)
    {
        var tableFirmasOrden = new Table(3).UseAllAvailableWidth();
        tableFirmasOrden.SetBorder(Border.NO_BORDER);
        tableFirmasOrden.SetHorizontalAlignment(HorizontalAlignment.CENTER);
        tableFirmasOrden.SetVerticalAlignment(VerticalAlignment.MIDDLE);
        tableFirmasOrden.SetMarginTop(40);
        var styleCellFirmasOrden = new Style()
            .SetBorder(Border.NO_BORDER)
            .SetTextAlignment(TextAlignment.CENTER);

        var cellFirmas = new Cell()
            .Add(
                new Paragraph("__________________________").AddStyle(GetStyleReportes()["styleFirmasOrdenTitle"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);
        cellFirmas = new Cell()
            .Add(
                new Paragraph("__________________________").AddStyle(GetStyleReportes()["styleFirmasOrdenTitle"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);
        cellFirmas = new Cell()
            .Add(
                new Paragraph("__________________________").AddStyle(GetStyleReportes()["styleFirmasOrdenTitle"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);
        cellFirmas = new Cell()
            .Add(new Paragraph("INGRESADO POR:").AddStyle(GetStyleReportes()["styleFirmasOrdenTitle"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);
        cellFirmas = new Cell()
            .Add(new Paragraph("REVISADO POR:").AddStyle(GetStyleReportes()["styleFirmasOrdenTitle"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);
        cellFirmas = new Cell()
            .Add(new Paragraph("APROBADO POR:").AddStyle(GetStyleReportes()["styleFirmasOrdenTitle"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);
        cellFirmas = new Cell()
            .Add(new Paragraph(ingresa).AddStyle(GetStyleReportes()["styleFirmasOrdenValue"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);
        cellFirmas = new Cell()
            .Add(new Paragraph(revisa).AddStyle(GetStyleReportes()["styleFirmasOrdenValue"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);
        cellFirmas = new Cell()
            .Add(new Paragraph(autoriza).AddStyle(GetStyleReportes()["styleFirmasOrdenValue"]))
            .AddStyle(styleCellFirmasOrden);
        tableFirmasOrden.AddCell(cellFirmas);

        return tableFirmasOrden;
    }

    private static Dictionary<string, Style> GetStyleReportes()
    {
        var stylesAutorizacion = new Dictionary<string, Style>();


        var titleHeaderStyle = new Style()
            .SetFontSize(10)
            .SetBold()
            .SetMarginTop(4)
            .SetTextAlignment(TextAlignment.CENTER);
        var subtitleHeaderStyle = new Style()
            .SetFontSize(9)
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginLeft(10f)
            .SetMarginRight(10f);

        var styleContent = new Style()
            .SetFontSize(8)
            .SetTextAlignment(TextAlignment.JUSTIFIED)
            .SetBorder(Border.NO_BORDER);
        var styleContentBold = new Style()
            .SetFontSize(8)
            .SetBold()
            .SetTextAlignment(TextAlignment.JUSTIFIED)
            .SetBorder(Border.NO_BORDER);

        var styleFirmasContent = new Style()
            .SetFontSize(7)
            .SetTextAlignment(TextAlignment.CENTER);

        var styleFirmasContentBold = new Style()
            .SetFontSize(7)
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER);

        var styleTitleCuadre = new Style()
            .SetFontSize(11)
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginTop(10f)
            .SetBorder(Border.NO_BORDER);

        var styleSubTitleCuadre = new Style()
            .SetFontSize(9)
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(Border.NO_BORDER);

        var styleSubTitle2Cuadre = new Style()
            .SetFontSize(9)
            .SetBold()
            .SetItalic()
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(Border.NO_BORDER);

        var styleCellCuadreBody = new Style()
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
            .SetTextAlignment(TextAlignment.CENTER);

        var styleCellCuadreBodyValue = new Style()
            .SetTextAlignment(TextAlignment.CENTER);

        var styleTitleCuadreCellBody = new Style()
            .SetFontSize(7)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold();

        var styleValueCuadreCellBody = new Style()
            .SetFontSize(7)
            .SetTextAlignment(TextAlignment.CENTER);

        var styleFirmaValue = new Style()
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(6);

        var styleCellFirma = new Style()
            .SetBorder(Border.NO_BORDER)
            .SetTextAlignment(TextAlignment.CENTER);

        var styleTitleDetalleCuadre = new Style()
            .SetFontSize(8)
            .SetTextAlignment(TextAlignment.LEFT)
            .SetBold()
            .SetMarginTop(10f);

        var styleOrdenTitle = new Style()
            .SetFontSize(9)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold();

        var styleOrdenSubTitle = new Style()
            .SetFontSize(8)
            .SetTextAlignment(TextAlignment.LEFT)
            .SetBold();

        var styleOrdenValue = new Style()
            .SetFontSize(8)
            .SetTextAlignment(TextAlignment.RIGHT);

        var styleOrdenDetalle = new Style()
            .SetFontSize(8)
            .SetTextAlignment(TextAlignment.JUSTIFIED);

        var styleFirmasOrdenValue = new Style()
            .SetFontSize(7)
            .SetTextAlignment(TextAlignment.CENTER);

        var styleFirmasOrdenTitle = new Style()
            .SetFontSize(7)
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER);


        stylesAutorizacion.Add("titleHeaderStyle", titleHeaderStyle);
        stylesAutorizacion.Add("subtitleHeaderStyle", subtitleHeaderStyle);
        stylesAutorizacion.Add("styleContent", styleContent);
        stylesAutorizacion.Add("styleContentBold", styleContentBold);
        stylesAutorizacion.Add("styleFirmasContent", styleFirmasContent);
        stylesAutorizacion.Add("styleFirmasContentBold", styleFirmasContentBold);
        stylesAutorizacion.Add("styleTitleCuadre", styleTitleCuadre);
        stylesAutorizacion.Add("styleSubTitleCuadre", styleSubTitleCuadre);
        stylesAutorizacion.Add("styleSubTitle2Cuadre", styleSubTitle2Cuadre);
        stylesAutorizacion.Add("styleCellCuadreBody", styleCellCuadreBody);
        stylesAutorizacion.Add("styleCellCuadreBodyValue", styleCellCuadreBodyValue);
        stylesAutorizacion.Add("styleTitleCuadreCellBody", styleTitleCuadreCellBody);
        stylesAutorizacion.Add("styleValueCuadreCellBody", styleValueCuadreCellBody);
        stylesAutorizacion.Add("styleFirmaValue", styleFirmaValue);
        stylesAutorizacion.Add("styleCellFirma", styleCellFirma);
        stylesAutorizacion.Add("styleTitleDetalleCuadre", styleTitleDetalleCuadre);
        stylesAutorizacion.Add("styleOrdenTitle", styleOrdenTitle);
        stylesAutorizacion.Add("styleOrdenSubTitle", styleOrdenSubTitle);
        stylesAutorizacion.Add("styleOrdenValue", styleOrdenValue);
        stylesAutorizacion.Add("styleOrdenDetalle", styleOrdenDetalle);
        stylesAutorizacion.Add("styleFirmasOrdenValue", styleFirmasOrdenValue);
        stylesAutorizacion.Add("styleFirmasOrdenTitle", styleFirmasOrdenTitle);


        return stylesAutorizacion;
    }
}