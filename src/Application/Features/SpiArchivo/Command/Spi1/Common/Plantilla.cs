using Domain.Entities.SpiArchivo;

namespace Application.Features.SpiArchivo.Command.Spi1.Common;

public static class Plantilla
{
    public static string GenerarPlantillaConsolidado(CabeceraSpi1 cabeceraSpi1, IReadOnlyList<DetalleSpi1Consolidado> lstConsolidadoSpi1)
    {
        var clientes = lstConsolidadoSpi1.Where( x => x.int_tipo_ordenante == 1 ).ToList();
        var proveedores = lstConsolidadoSpi1.Where( x => x.int_tipo_ordenante == 2 ).ToList();
        var interbancarias = lstConsolidadoSpi1.Where( x => x.int_tipo_ordenante == 3 ).ToList();
        var totalClientes = clientes.Sum( x => Convert.ToDecimal( x.dec_monto_credito_opi ) );
        var totalProveedores = proveedores.Sum( x => Convert.ToDecimal( x.dec_monto_credito_opi ) );
        var totalInterbancarias = interbancarias.Sum( x => Convert.ToDecimal( x.dec_monto_credito_opi ) );
        var decTotalOpis = totalClientes + totalProveedores + totalInterbancarias;

        var plantilla = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
        plantilla += "<html><plantilla style=\"font-family: 'Gill Sans MT'; font-size: small;\"><p><b>REPORTE CONSOLIDADO DE PAGOS Y  TRANSFERENCIAS SPI</b></p>";
        plantilla += "<table border=\"0\" cellpadding=\"2\" cellspacing=\"2\" style=\"font-family: 'Gill Sans MT'\">";
        plantilla += "<tr><td><b>N&uacute;mero de Corte:</b></td><td>" + cabeceraSpi1.int_numero_envio + "</td></tr>";
        plantilla += "<tr><td><b>Fecha de Env&iacute;o:</b></td><td>" + Convert.ToDateTime( cabeceraSpi1.dtt_fecha_envio ).ToString( "MM/dd/yyyy HH:mm:ss" ) + "</td></tr>";
        plantilla += "<tr><td><b>Total de Registros:</b></td><td>" + cabeceraSpi1.int_num_total_opi + "</td></tr>";
        plantilla += "<tr><td><b>Monto Total:</b></td><td>&#36; " + cabeceraSpi1.dec_monto_total_opi + "</td></tr>";
        plantilla += "<tr><td><b>Nombre del Archivo:</b></td><td>" + cabeceraSpi1.str_nom_archivo_spi1 + "</td></tr>";
        plantilla += "<tr><td>&nbsp;</td><td>&nbsp;</td></tr></table>";

        plantilla += "<table border=\"1\" cellpadding=\"2\" cellspacing=\"2\" style=\"font-family: 'Gill Sans MT'; font-size: x-small;\">";
        plantilla += "<tr><td colspan=\"8\"><b>DETALLE CLIENTES</b></td></tr>";
        plantilla += "<tr style=\"text-align: center; background-color: #004cac; color: white;\"><td>Fecha</td><td>Ingresado</td><td>Referencia</td><td>Cuenta D&eacute;bito</td><td>Ordenante</td><td>Cuenta Destino</td><td>Banco Destino</td><td>Monto</td></tr>";
        foreach (var consolidado in clientes)
        {
            plantilla += "<tr style=\"text-align: center;\"><td>" + consolidado.dtt_fecha_registro_io.ToString( "MM/dd/yyyy HH:mm:ss" ) + "</td>";
            plantilla += "<td>" + consolidado.str_usuario + "</td>";
            plantilla += "<td>" + consolidado.str_num_referencia + "</td>";
            plantilla += "<td>" + consolidado.str_cuenta_co_io + "</td>";
            plantilla += "<td>" + consolidado.str_nombre_co + "</td>";
            plantilla += "<td>" + consolidado.str_cuenta_cb_ir + "</td>";
            plantilla += "<td>" + consolidado.str_banco + "</td>";
            plantilla += "<td style=\"text-align: right;\">" + consolidado.dec_monto_credito_opi + "</td></tr>";
        }
        plantilla += "<tr style=\"text-align: right; background-color: #95b0ec;\"><td colspan=\"7\"><b>SUBTOTAL:</b></td><td><b>" + totalClientes + "</b></td></tr>";
        if (proveedores.Count > 0)
        {
            plantilla += "<tr><td colspan=\"8\"><b>DETALLE PAGOS</b></td></tr>";
            plantilla += "<tr style=\"text-align: center; background-color: #004cac; color: white;\"><td>Fecha Ingreso</td><td>Ingresado</td><td>Referencia</td><td>ID Proveedor</td><td>Proveedor</td><td>Cuenta Destino</td><td>Banco Destino</td><td>Valor</td></tr>";
            foreach (var consolidado in proveedores)
            {
                plantilla += "<tr style=\"text-align: center;\"><td>" + consolidado.dtt_fecha_registro_io.ToString( "MM/dd/yyyy HH:mm:ss" ) + "</td>";
                plantilla += "<td>" + consolidado.str_usuario + "</td>";
                plantilla += "<td>" + consolidado.str_num_referencia + "</td>";
                plantilla += "<td>" + consolidado.str_ced_ruc_cb + "</td>";
                plantilla += "<td>" + consolidado.str_nombre_co + "</td>";
                plantilla += "<td>" + consolidado.str_cuenta_cb_ir + "</td>";
                plantilla += "<td>" + consolidado.str_banco + "</td>";
                plantilla += "<td style=\"text-align: right;\">" + consolidado.dec_monto_credito_opi + "</td></tr>";
            }
            plantilla += "<tr style=\"text-align: right; background-color: #95b0ec;\"><td colspan=\"7\"><b>SUBTOTAL:</b></td><td><b>" + totalProveedores + "</b></td></tr>";
        }

        if (interbancarias.Count > 0)
        {
            plantilla += "<tr><td colspan=\"8\"><b>DETALLE INTERBANCARIAS</b></td></tr>";
            plantilla += "<tr style=\"text-align: center; background-color: #004cac; color: white;\"><td>Fecha Ingreso</td><td>Ingresado</td><td>Referencia</td><td>Cuenta D&eacute;bito</td><td>Ordenante</td><td>Cuenta Destino</td><td>Banco Destino</td><td>Valor</td></tr>";
            foreach (var consolidado in interbancarias)
            {
                plantilla += "<tr style=\"text-align: center;\"><td>" + consolidado.dtt_fecha_registro_io.ToString( "MM/dd/yyyy HH:mm:ss" ) + "</td>";
                plantilla += "<td>" + consolidado.str_usuario + "</td>";
                plantilla += "<td>" + consolidado.str_num_referencia + "</td>";
                plantilla += "<td>" + consolidado.str_cuenta_co_io + "</td>";
                plantilla += "<td>" + consolidado.str_nombre_co + "</td>";
                plantilla += "<td>" + consolidado.str_cuenta_cb_ir + "</td>";
                plantilla += "<td>" + consolidado.str_banco + "</td>";
                plantilla += "<td style=\"text-align: right;\">" + consolidado.dec_monto_credito_opi + "</td></tr>";
            }
            plantilla += "<tr style=\"text-align: right; background-color: #95b0ec;\"><td colspan=\"7\"><b>SUBTOTAL:</b></td><td><b>" + totalInterbancarias + "</b></td></tr>";
        }
        plantilla += "<tr style=\"text-align: right;\"><td colspan=\"7\"><b>TOTAL:</b></td><td><b>" + decTotalOpis + "</b></td></tr></table>";
        plantilla += "</body></html>";

        return plantilla;
    }
}