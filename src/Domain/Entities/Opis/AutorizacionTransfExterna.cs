namespace Domain.Entities.Opis;

public class AutorizacionTransfExterna
{
    public string str_mensaje { get; set; } = string.Empty;
    public string str_concepto { get; set; } = string.Empty;
    public string str_cedula_ruc { get; set; } = string.Empty;
    public string str_nombre_ord { get; set; } = string.Empty;
    public string str_num_cuenta_ord { get; set; } = string.Empty;
    public string str_cedula_ruc_benef { get; set; } = string.Empty;
    public string str_razon_social_benef { get; set; } = string.Empty;
    public string str_num_cuenta_benef { get; set; } = string.Empty;
    public string str_monto_opi { get; set; } = string.Empty;
    public string str_observaciones { get; set; } = string.Empty;
    public string str_banco_benef { get; set; } = string.Empty;
    public string str_tipo_cuenta_benef { get; set; } = string.Empty;
    public string str_usuario { get; set; } = string.Empty;
    public string str_oficina { get; set; } = string.Empty;
    public int int_codigo_opi { get; set; }
    public string str_representante { get; set; } = string.Empty;
}