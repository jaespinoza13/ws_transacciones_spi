using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Opis.Queries.Buscar;

public class ReqBuscarOpis : Header, IRequest<ResBuscarOpis>
{
    public DateTime dtt_fecha_desde { get; set; }
    public DateTime dtt_fecha_hasta { get; set; }
    public string str_tipo_transf { get; set; } = "-1";
    public string str_cta_ordenante { get; set; } = "-1";
    public string str_ident_ordenante { get; set; } = "-1";
    public string str_cta_beneficiario { get; set; } = "-1";
    public string str_ident_beneficiario { get; set; } = "-1";
    public int int_codigo_opi { get; set; } = -1;
    public string str_usuario { get; set; } = "-1";
    public decimal dec_monto { get; set; } = -1;
    public int int_nivel_opi { get; set; } = -1;
    public int int_estado_bce { get; set; } = -1;
    public int int_estado_banred { get; set; } = -1;
    public string str_banco_destino { get; set; } = "-1";
    public string str_ruc_proveedor { get; set; } = "-1";
    public string str_comprobante_venta { get; set; } = "-1";
    public string str_comprobante_cont { get; set; } = "-1";
}