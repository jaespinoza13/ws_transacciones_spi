using System.ComponentModel;

namespace Application.Features.Opis.Vm;

public class TransferenciaReporteVm
{
    [DisplayName( "No. Referencia" )] public int int_codigo_opi { get; set; }
    [DisplayName( "Fecha Ingreso" )] public string dtt_fecha_ingresa { get; set; } = string.Empty;
    [DisplayName( "Usuario Ingresa" )] public string str_usuario_ingresa { get; set; } = string.Empty;
    [DisplayName( "Oficina Ingresa" )] public string str_oficina { get; set; } = string.Empty;
    [DisplayName( "Id Ordenante" )] public string str_ident_ordenante { get; set; } = string.Empty;
    [DisplayName( "Nombre Ordenante" )] public string str_nombre_ordenante { get; set; } = string.Empty;
    [DisplayName( "Cuenta Ordenante" )] public string str_cta_ordenante { get; set; } = string.Empty;
    [DisplayName( "Monto" )] public decimal dec_monto_opi { get; set; }
    [DisplayName( "Id Beneficiario" )] public string str_ident_beneficiario { get; set; } = string.Empty;
    [DisplayName( "Nombre Beneficiario" )] public string str_nombre_beneficiario { get; set; } = string.Empty;
    [DisplayName( "Cuenta Beneficiario" )] public string str_cta_beneficiario { get; set; } = string.Empty;
    [DisplayName( "Banco Beneficiario" )] public string str_banco_benef { get; set; } = string.Empty;
    [DisplayName( "Observaciones" )] public string str_observaciones { get; set; } = string.Empty;
    [DisplayName( "Canal Envío" )] public string str_tipo_transf { get; set; } = string.Empty;
    [DisplayName( "Estado Interno" )] public string str_estado_opi { get; set; } = string.Empty;
    [DisplayName( "Estado" )] public string str_estado { get; set; } = string.Empty;
}