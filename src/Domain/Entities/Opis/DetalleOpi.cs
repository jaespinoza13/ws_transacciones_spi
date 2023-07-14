namespace Domain.Entities.Opis;

public class DetalleOpi
{
    public int int_codigo_opi { get; set; }
    public string str_ident_ordenante { get; set; } = string.Empty;
    public string str_nombre_ordenante { get; set; } = string.Empty;
    public string str_cta_ordenante { get; set; } = string.Empty;
    public string str_estado_cta_ordenante { get; set; } = string.Empty;
    public string str_tipo_per_ordenante { get; set; } = string.Empty;
    public string str_tipo_cta_ordenante { get; set; } = string.Empty;
    public string str_ident_beneficiario { get; set; } = string.Empty;
    public string str_nombre_beneficiario { get; set; } = string.Empty;
    public string str_nombre_destino { get; set; } = string.Empty;
    public string str_cta_beneficiario { get; set; } = string.Empty;
    public string str_tipo_cta_beneficiario { get; set; } = string.Empty;
    public decimal dec_monto { get; set; }
    public string str_observaciones { get; set; } = string.Empty;
    public string str_estado_interno { get; set; } = string.Empty;
    public string str_info_adicional { get; set; } = string.Empty;
    public string str_usuario_nomb_ingresa { get; set; } = string.Empty;
    public string str_usuario_revisa_1 { get; set; } = string.Empty;
    public string str_usuario_nomb_aprueba_1 { get; set; } = string.Empty;
    public string str_usuario_nomb_revisa { get; set; } = string.Empty;
    public string str_nombre_comercial_ord { get; set; } = string.Empty;
    public string str_nombre_cta_beneficiario { get; set; } = string.Empty;
    public string str_tipo_comprobante { get; set; } = string.Empty;
    public string str_num_combrobante { get; set; } = string.Empty;
    public string str_oficina_origen { get; set; } = string.Empty;
    public string str_tramitado { get; set; } = string.Empty;
    public decimal dec_valor_factura { get; set; }
    public string str_num_retencion { get; set; } = string.Empty;
    public string str_planilla_contable { get; set; } = string.Empty;
    public string str_solicitud_pago { get; set; } = string.Empty;
    public string str_usuario_ingresa { get; set; } = string.Empty;
    public string dtt_fecha_ingresa { get; set; } = string.Empty;
    public string str_usuario_revisa { get; set; } = string.Empty;
    public string dtt_fecha_revisa { get; set; } = string.Empty;
    public string str_usuario_revisa_2 { get; set; } = string.Empty;
    public string str_usuario_nomb_rechaza { get; set; } = string.Empty;
    public string dtt_fecha_revisa_2 { get; set; } = string.Empty;
    public string str_usuario_aprueba { get; set; } = string.Empty;
    public string dtt_fecha_aprueba { get; set; } = string.Empty;
    public string str_usuario_aprueba_2 { get; set; } = string.Empty;
    public string dtt_fecha_aprueba_2 { get; set; } = string.Empty;
    public string str_usuario_rechaza { get; set; } = string.Empty;
    public string dtt_fecha_rechaza { get; set; } = string.Empty;
    public string str_motivo_rechazo { get; set; } = string.Empty;
    public string str_ruta_archivo { get; set; } = string.Empty;
    public string str_tipo_ordenante { get; set; } = string.Empty;
}