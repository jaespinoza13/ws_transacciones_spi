using Application.Features.Opis.Vm;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace Application.Features.Opis.Queries.Buscar.Common;

public static class ReporteFile
{
    public static string GenerarReporteTransferencias(ReqBuscarOpis request, IReadOnlyList<TransferenciaReporteVm> opis)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add( $"Reporte_{request.str_tipo_transf}_{DateTime.Now:dd/MM/yyyy}" );
        
        var titleCell = worksheet.Cells["A1:D1"];
        titleCell.Merge = true;
        titleCell.Value = "REPORTE DE TRANSFERENCIAS INTERBANCARIAS";
        titleCell.Style.Font.Size = 16;
        titleCell.Style.Font.Bold = true;
        titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        
        worksheet.Cells["A3"].Value = "Fecha Desde: ";
        worksheet.Cells["A4"].Value = "Fecha Hasta: ";
        worksheet.Cells["A3:A4"].Style.Font.Bold = true;
        worksheet.Cells["B3:B4"].Value = new object[] { request.dtt_fecha_desde.ToString( "dd/MM/yyyy" ), request.dtt_fecha_hasta.ToString( "dd/MM/yyyy" ) };

        var headerRow = worksheet.Cells["A6"].LoadFromCollection( opis, true, TableStyles.Light9 );
        headerRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        headerRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        headerRow.AutoFilter = false;
        
        var montoColumn = headerRow.FirstOrDefault( cell => cell.Text.Equals( "Monto", StringComparison.OrdinalIgnoreCase ) );
        if (montoColumn != null)
        {
            montoColumn.EntireColumn.Style.Numberformat.Format = "#,##0.00";
        }
        
        var lastRow = opis.Count + 6;
        var sumCell = worksheet.Cells[lastRow + 1, 8];
        sumCell.Formula = $"SUM(H7:H{lastRow})";
        sumCell.Style.Numberformat.Format = "#,##0.00";
        sumCell.Style.Font.Bold = true;
        sumCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
        sumCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        sumCell.Style.Font.Size = 11;
        sumCell.Calculate();
        sumCell.AutoFitColumns();
        worksheet.Cells.AutoFitColumns();
        
        using var stream = new MemoryStream();
        package.SaveAs( stream );

        return Convert.ToBase64String( stream.ToArray() );
    }
    
    public static string GenerarReporteProveedores(ReqBuscarOpis request, IReadOnlyList<ProveedorReporteVm> opis)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add( $"Reporte_{request.str_tipo_transf}_{DateTime.Now:dd/MM/yyyy}" );

        var titleCell = worksheet.Cells["A1:D1"];
        titleCell.Merge = true;
        titleCell.Value = "REPORTE DE TRANSFERENCIAS PROVEEDORES";
        titleCell.Style.Font.Size = 16;
        titleCell.Style.Font.Bold = true;
        titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        worksheet.Cells["A3"].Value = "Fecha Desde: ";
        worksheet.Cells["A4"].Value = "Fecha Hasta: ";
        worksheet.Cells["A3:A4"].Style.Font.Bold = true;
        worksheet.Cells["B3:B4"].Value = new object[] { request.dtt_fecha_desde.ToString( "dd/MM/yyyy" ), request.dtt_fecha_hasta.ToString( "dd/MM/yyyy" ) };
        
        var headerRow = worksheet.Cells["A6"].LoadFromCollection( opis, true, TableStyles.Light9 );
        headerRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        headerRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        headerRow.AutoFilter = false;

        var montoColumn = headerRow.FirstOrDefault( cell => cell.Text.Equals( "Monto", StringComparison.OrdinalIgnoreCase ) );
        if (montoColumn != null)
        {
            montoColumn.EntireColumn.Style.Numberformat.Format = "#,##0.00";
        }
        
        var lastRow = opis.Count + 6;
        var sumCell = worksheet.Cells[lastRow + 1, 8];
        sumCell.Formula = $"SUM(H7:H{lastRow})";
        sumCell.Style.Numberformat.Format = "#,##0.00";
        sumCell.Style.Font.Bold = true;
        sumCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        sumCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        sumCell.Style.Font.Size = 11;
        sumCell.Calculate();
        sumCell.AutoFitColumns();
        worksheet.Cells.AutoFitColumns();
        
        using var stream = new MemoryStream();
        package.SaveAs( stream );

        return Convert.ToBase64String( stream.ToArray() );
    }

}