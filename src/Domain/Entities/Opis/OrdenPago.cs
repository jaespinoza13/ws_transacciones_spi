namespace Domain.Entities.Opis;

public class OrdenPago
{
    public int int_codigo_opi { get; set; }
    public string str_cedula_ordenante { get; set; } = string.Empty;
    public string str_nombre_ordenante { get; set; } = string.Empty;
    public string str_cuenta_ordenante { get; set; } = string.Empty;
    public string str_cedula_beneficiario { get; set; } = string.Empty;
    public string str_nombre_beneficiario { get; set; } = string.Empty;
    public string str_cuenta_beneficiario { get; set; } = string.Empty;
    public string dec_monto { get; set; } = string.Empty;
    public string dtt_fecha_ingresa { get; set; } = string.Empty;
    public string str_usuario_ingresa { get; set; } = string.Empty;
    public string str_tipo_transaccion { get; set; } = string.Empty;
    public string str_tipo_ordenante { get; set; } = string.Empty;
    public string str_estado_interno { get; set; } = string.Empty;
    public string str_banco_destino { get; set; } = string.Empty;
}