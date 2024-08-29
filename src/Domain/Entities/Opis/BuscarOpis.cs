namespace Domain.Entities.Opis;

public class BuscarOpis
{
    public int int_codigo_opi { get; set; }
    public string str_ident_ordenante { get; set; } = string.Empty;
    public string str_nombre_ordenante { get; set; } = string.Empty;
    public string str_cta_ordenante { get; set; } = string.Empty;
    public string str_ident_beneficiario { get; set; } = string.Empty;
    public string str_nombre_beneficiario { get; set; } = string.Empty;
    public string str_cta_beneficiario { get; set; } = string.Empty;
    public string dec_monto_opi { get; set; } = string.Empty;
    public string dtt_fecha_ingresa { get; set; } = string.Empty;
    public string str_usuario_ingresa { get; set; } = string.Empty;
    public string str_tipo_ord { get; set; } = string.Empty;
    public string str_banco_benef { get; set; } = string.Empty;
    public string str_oficina { get; set; } = string.Empty;
    public string str_estado_opi { get; set; } = string.Empty;
    public string str_estado { get; set; } = string.Empty;
    public string str_observaciones { get; set; } = string.Empty;
    public string str_tipo_transf { get; set; } = string.Empty;
    public string str_num_comprobante { get; set; } = string.Empty;
    public string str_planilla_contable { get; set; } = string.Empty;
    public string str_usuario_rechaza { get; set; } = string.Empty;
    public string str_usuario_aprueba { get; set; } = string.Empty;

}