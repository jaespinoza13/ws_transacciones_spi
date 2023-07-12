namespace Domain.Entities.Spi1;

public class DetalleSpi1
{
    public int int_codigo_spi1 { get; set; }
    public DateTime dtt_fecha_registro_io { get; set; }
    public string str_num_referencia { get; set; } = string.Empty;
    public int int_cod_origen_io { get; set; }
    public int int_cod_moneda { get; set; }
    public string dec_monto_credito_opi { get; set; }  = string.Empty;
    public int int_cod_concepto_opi { get; set; }
    public string str_cuenta_bce_ir { get; set; } = string.Empty;
    public string str_cuenta_co_io { get; set; } = string.Empty;
    public int int_tipo_cuenta_co { get; set; }
    public string str_nombre_co { get; set; } = string.Empty;
    public string str_lugar_opi_io { get; set; } = string.Empty;
    public string str_cuenta_cb_ir { get; set; } = string.Empty;
    public int int_tipo_cuenta_cb { get; set; }
    public string str_nombre_cb { get; set; } = string.Empty;
    public string str_info_adicional_opi { get; set; } = string.Empty;
    public string str_ced_ruc_cb { get; set; } = string.Empty;
    public int int_estado_opi { get; set; }
    
}